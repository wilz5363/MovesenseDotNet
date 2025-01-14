﻿using MdsLibrary.Helpers;
using Plugin.Movesense;
using Plugin.Movesense.Api;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
#if __IOS__
using Foundation;
#endif

namespace MdsLibrary.Api
{
    /// <summary>
    /// Makes a subscription to an MdsLib resource
    /// </summary>
    public class ApiSubscription<T> : 
#if __ANDROID__
             Java.Lang.Object, Com.Movesense.Mds.IMdsNotificationListener,
#endif
        IApiSubscription<T>
    {
        private static readonly int RETRY_DELAY = 5000; //5 sec
        private static int MAX_RETRY_COUNT = 2;
        private int retries = 0;
        private readonly string mSerial;
        private readonly string mPath;
        TaskCompletionSource<IMdsSubscription> mTcs;
        Action<T> mNotificationCallback;
#if __ANDROID__
        private static readonly string URI_EVENTLISTENER = "suunto://MDS/EventListener";
#endif

        /// <summary>
        /// The context for the subscription
        /// </summary>
        public IMdsSubscription Subscription { get; private set; }

        /// <summary>
        /// Utility class for API subscriptions
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="path">The path of the MdsLib resource, for example "/Meas/Acc/52" to subscribe to accelerometer at 52Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiSubscription(MdsConnectionContext, string) constructor instead.")]
        public ApiSubscription(string deviceName, string path)
        {
            if (string.IsNullOrEmpty(deviceName)) throw new InvalidOperationException("Required parameter deviceName must have value");
            if (string.IsNullOrEmpty(path)) throw new InvalidOperationException("Required parameter path must have value");

            mSerial = Util.GetVisibleSerial(deviceName);
            mPath = path;
            if (mPath.Substring(0, 1) != "/")
            {
                mPath = "/" + mPath;
            }

            // Define the built-in implementation of the retry function
            // This just retries 2 times, regardless of the exception thrown
            // The user may provide their own implementation of the Retry function to override this behavior
            RetryFunction = new Func<Exception, bool?>((Exception ex) =>
            {
                bool? cancel = false;
                if (++retries > MAX_RETRY_COUNT)
                {
                    cancel = true;
                }
                return cancel;
            }
        );
        }

        /// <summary>
        /// Utility class for API subscriptions
        /// </summary>
        /// <param name="movesenseDevice">IMovesenseDevice for the device</param>
        /// <param name="path">The path of the MdsLib resource, for example "/Meas/Acc/52" to subscribe to accelerometer at 52Hz</param>
        public ApiSubscription(IMovesenseDevice movesenseDevice, string path)
        {
            if (movesenseDevice is null) throw new InvalidOperationException("Required parameter connectionContext must have value");
            if (string.IsNullOrEmpty(path)) throw new InvalidOperationException("Required parameter path must have value");

            mSerial = movesenseDevice.Serial;
            mPath = path;
            if (mPath.Substring(0, 1) != "/")
            {
                mPath = "/" + mPath;
            }

            // Define the built-in implementation of the retry function
            // This just retries 2 times, regardless of the exception thrown
            // The user may provide their own implementation of the Retry function to override this behavior
            RetryFunction = new Func<Exception, bool?>((Exception ex) =>
            {
                bool? cancel = false;
                if (++retries > MAX_RETRY_COUNT)
                {
                    cancel = true;
                }
                return cancel;
            }
        );
        }

        /// <summary>
        /// Retry function, called after the function call fails.
        /// The built-in implementation retries 2 times, regardless of the exception thrown.
        /// Override the built-in implementation by setting this to your own implementation of the Retry function
        /// </summary>
        public Func<Exception, bool?> RetryFunction;

        /// <summary>
        /// Subscribe to the resource
        /// </summary>
        /// <param name="notificationCallback">Callback function that will receive periodic notifications with data from the subscription resource</param>
        /// <returns>The subscription context</returns>
        public async Task<IMdsSubscription> SubscribeWithRetryAsync(Action<T> notificationCallback)
        {
            TaskCompletionSource<IMdsSubscription> retryTcs = new TaskCompletionSource<IMdsSubscription>();
            IMdsSubscription result = null;
            bool doRetry = true;
            while (doRetry)
            {
                try
                {
                    result = await doSubscribe(notificationCallback);
                    retryTcs.SetResult(result);
                    doRetry = false;
                }
                catch (Exception ex)
                {
                    bool? mCancelled = RetryFunction?.Invoke(ex);
                    if (mCancelled.HasValue && mCancelled.Value)
                    {
                        // User has cancelled - break out of loop
                        Debug.WriteLine($"MAX RETRY COUNT EXCEEDED giving up Mds Api Call after exception: {ex.ToString()}");
                        retryTcs.SetException(ex);
                        throw ex;
                    }
                    else
                    {
                        Debug.WriteLine($"RETRYING Mds Api Call after exception: {ex.ToString()}");
                        await Task.Delay(RETRY_DELAY);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Subscribe to the resource
        /// </summary>
        /// <param name="notificationCallback">Callback function that will receive periodic notifications with data from the subscription resource</param>
        /// <returns>The subscription context</returns>
        public Task<IMdsSubscription> SubscribeAsync(Action<T> notificationCallback)
        {
            return doSubscribe(notificationCallback);
        }

        /// <summary>
        /// Unsubscribe from the MdsLib resource
        /// </summary>
        public void UnSubscribe()
        {
            Debug.WriteLine("Unsubscribing Mds api subscription");
            Subscription?.Unsubscribe();
            Subscription = null;
        }

        private Task<IMdsSubscription> doSubscribe(Action<T> notificationCallback)
        {
            mTcs = new TaskCompletionSource<IMdsSubscription>();
            mNotificationCallback = notificationCallback;

#if __ANDROID__
            var mds = (Com.Movesense.Mds.Mds)CrossMovesense.Current.MdsInstance;
            var subscription = mds.Subscribe(
                URI_EVENTLISTENER, 
                FormatContractToJson(mSerial, mPath), 
                this
                );
            Subscription = new MdsSubscription(subscription);
#elif __IOS__
            var mds = (Movesense.MDSWrapper)CrossMovesense.Current.MdsInstance;
            Movesense.MDSResponseBlock responseBlock = new Movesense.MDSResponseBlock((arg0) => OnSubscribeCompleted(arg0));
            Movesense.MDSEventBlock eventBlock = (Movesense.MDSEvent arg0) => OnSubscriptionEvent(arg0);

            string path = mSerial + mPath;
            mds.DoSubscribe(path, new Foundation.NSDictionary(), responseBlock, eventBlock);
            // Save the path to the subscription for the device in the MdsSubscription
            Subscription = new MdsSubscription(path);
#endif

            return mTcs.Task;
        }

        private string FormatContractToJson(string serial, string uri)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"Uri\": \"");
            sb.Append(serial);
            sb.Append(uri);
            sb.Append("\"}");
            return sb.ToString();
        }

#if __ANDROID__

        /// <summary>
        /// Callback method that Mds calls with subscription data
        /// </summary>
        /// <param name="s"></param>
        public void OnNotification(string s)
        {
            Debug.WriteLine($"NOTIFICATION data = {s}");
            if (typeof(T) != typeof(String))
            {
                T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
                // Return the subscription to the awaiting caller
                mTcs.TrySetResult(Subscription);
                // Invoke the callers callback function
                mNotificationCallback?.Invoke(result);
            }
            else
            {
                // Crazy code to convert a string to a 'T' where 'T' happens to be a string
                T result = (T)((object)s);
                // Return the subscription to the awaiting caller
                mTcs.TrySetResult(Subscription);
                // Invoke the callers callback function
                mNotificationCallback?.Invoke(result);
            }
        }

        /// <summary>
        /// Error callback called by Mds when an error is encoubntered reading subscription data
        /// </summary>
        /// <param name="e"></param>
        public void OnError(Com.Movesense.Mds.MdsException e)
        {
            Debug.WriteLine($"ERROR error = {e.ToString()}");
            mTcs.SetException(new MdsException(e.ToString(), e));
        }

#elif __IOS__

        private void OnSubscribeCompleted(Movesense.MDSResponse response)
        {
            if (response.StatusCode == 200)
            {
                System.Diagnostics.Debug.WriteLine("Success subscription: " + response.Description);
                // Return the subscription to the awaiting caller
                mTcs.TrySetResult(Subscription);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to subscribe: " + response.Description);
                mTcs.SetException(new MdsException(response.Description));
            }
        }

        /// <summary>
        /// Callback method that Mds calls with subscription data
        /// </summary>
        /// <param name="mdsevent">Data for the subscription notification</param>
        public void OnSubscriptionEvent(Movesense.MDSEvent mdsevent)
        {
            var data = mdsevent.BodyData;
            NSString s = new NSString(data, NSStringEncoding.UTF8);
            Debug.WriteLine($"NOTIFICATION data = {s}");
            if (typeof(T) != typeof(String))
            {
                T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
                // Return the subscription to the awaiting caller
                mTcs.TrySetResult(Subscription);
                // Invoke the callers callback function
                mNotificationCallback?.Invoke(result);
            }
            else
            {
                // First convert NSString result to a .NET string
                String netS = string.Empty;
                netS = s?.ToString();
                // Crazy code to convert a string to a 'T' where 'T' happens to be a string
                T result = (T)((object)netS);
                // Return the subscription to the awaiting caller
                mTcs.TrySetResult(Subscription);
                // Invoke the callers callback function
                mNotificationCallback?.Invoke(result);
            }
        }
#endif

    }
}
