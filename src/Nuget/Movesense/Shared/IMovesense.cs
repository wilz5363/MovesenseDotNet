﻿using MdsLibrary.Model;
using Plugin.Movesense.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Movesense
{
    /// <summary>
    /// Movesense Plugin API
    /// </summary>
    public interface IMovesense
    {
        /// <summary>
        /// Gets the native MdsLib object
        /// </summary>
        object MdsInstance { get; }

        /// <summary>
        /// On Android, this must be set to the current Activity before first access of the library.
        /// </summary>
        object Activity { set; }

        /// <summary>
        /// Root of all paths to Movesense resources.
        /// </summary>
        string SCHEME_PREFIX { get; }

        /// <summary>
        /// Connect a device to MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>MdsConnectionContext for the device</returns>
        Task<MdsConnectionContext> ConnectMdsAsync(Guid Uuid);

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="Uuid">Uuid of the device</param>
        /// <returns>null</returns>
        [Obsolete("DisconnectMdsAsync(Guid) is deprecated, please use DisconnectMdsAsync(MdsConnectionContect) instead.")]
        Task<object> DisconnectMdsAsync(Guid Uuid);

        /// <summary>
        /// Disconnect a device from MdsLib
        /// </summary>
        /// <param name="connectionContext">Connection context of the device</param>
        /// <returns>null</returns>
        [Obsolete("DisconnectMdsAsync(Guid) is deprecated, please use DisconnectMdsAsync(MdsConnectionContext) instead.")]
        Task<object> DisconnectMdsAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync(MdsConnectionContext, ...) instead.")]
        Task ApiCallAsync(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "");

        /// <summary>
        /// Function to make Mds API call that does not return a value
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        Task ApiCallAsync(MdsConnectionContext connectionContext, MdsOp restOp, string path, string body = null, string prefixPath = "");

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync<T>(MdsConnectionContext, ...) instead.")]
        Task<T> ApiCallAsync<T>(string deviceName, MdsOp restOp, string path, string body = null, string prefixPath = "");

        /// <summary>
        /// Function to make Mds API call that returns a value of type T
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="restOp">The type of REST call to make to MdsLib</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="body">JSON body if any</param>
        /// <param name="prefixPath">optional prefix of the target URI before the device serial number (defaults to empty string)</param>
        Task<T> ApiCallAsync<T>(MdsConnectionContext connectionContext, MdsOp restOp, string path, string body = null, string prefixPath = "");

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiSubscriptionAsync<T>(MdsConnectionContext, ...) instead.")]
        Task<IMdsSubscription> ApiSubscriptionAsync<T>(string deviceName, string path, Action<T> notificationCallback);

        /// <summary>
        /// Function to start a subscription to an Mds resource
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="path">The path of the MdsLib resource</param>
        /// <param name="notificationCallback">Callback function that takes parameter of type T, where T is the return type from the subscription notifications</param>
        Task<IMdsSubscription> ApiSubscriptionAsync<T>(MdsConnectionContext connectionContext, string path, Action<T> notificationCallback);

        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <returns>new Log Id</returns>
        [Obsolete("Passing argument of deviceName is deprecated, please use ApiCallAsync(CreateLogEntryAsync) instead.")]
        Task<CreateLogResult> CreateLogEntryAsync(string deviceName);
        /// <summary>
        /// Create a new logbook entry resource (increment log Id). Returns the new log Id.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <returns>new Log Id</returns>
        Task<CreateLogResult> CreateLogEntryAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use DeleteLogEntriesAsync(MdsConnectionContext) instead.")]
        Task DeleteLogEntriesAsync(string deviceName);
        /// <summary>
        /// Delete all the Logbook entries
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task DeleteLogEntriesAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetAccInfoAsync(MdsConnectionContext) instead.")]
        Task<AccInfo> GetAccInfoAsync(string deviceName);
        /// <summary>
        /// Get Accelerometer configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<AccInfo> GetAccInfoAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetBatteryLevelAsync(MdsConnectionContext) instead.")]
        Task<BatteryResult> GetBatteryLevelAsync(string deviceName);

        /// <summary>
        /// Get Battery level, CallAsync returns BatteryResult
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<BatteryResult> GetBatteryLevelAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetAppInfoAsync(MdsConnectionContext) instead.")]
        Task<AppInfo> GetAppInfoAsync(string deviceName);
        /// <summary>
        /// Get info on the app running on the device
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<AppInfo> GetAppInfoAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get device info
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetDeviceInfoAsync(MdsConnectionContext) instead.")]
        Task<DeviceInfoResult> GetDeviceInfoAsync(string deviceName);

        /// <summary>
        /// Get device info
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<DeviceInfoResult> GetDeviceInfoAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetGyroInfoAsync(MdsConnectionContext) instead.")]
        Task<GyroInfo> GetGyroInfoAsync(string deviceName);

        /// <summary>
        /// Get Gyrometer configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<GyroInfo> GetGyroInfoAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetIMUInfoAsync(MdsConnectionContext) instead.")]
        Task<IMUInfo> GetIMUInfoAsync(string deviceName);

        /// <summary>
        /// Get IMU configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<IMUInfo> GetIMUInfoAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLedsStateAsync(MdsConnectionContext) instead.")]
        Task<LedsResult> GetLedsStateAsync(string deviceName);

        /// <summary>
        /// Get state of all Leds in the system
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<LedsResult> GetLedsStateAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Number of the Led</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLedStateAsync(MdsConnectionContext, int) instead.")]
        Task<LedState> GetLedStateAsync(string deviceName, int ledIndex = 0);

        /// <summary>
        /// Get LedState for an LED
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="ledIndex">Number of the Led</param>
        Task<LedState> GetLedStateAsync(MdsConnectionContext connectionContext, int ledIndex = 0);

        /// <summary>
        /// Get data from a Logbook entry in SBEM format by accessing the suunto://{serial}/Mem/Logbook/ByID/{ID}/Data REST endpoint
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Number of the entry to get</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogbookDataAsync(MdsConnectionContext, int) instead.")]
        Task<string> GetLogbookDataAsync(string deviceName, int logId);

        /// <summary>
        /// Get data from a Logbook entry in SBEM format by accessing the suunto://{serial}/Mem/Logbook/ByID/{ID}/Data REST endpoint
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="logId">Number of the entry to get</param>
        Task<string> GetLogbookDataAsync(MdsConnectionContext connectionContext, int logId);

        /// <summary>
        /// Get data from a Logbook entry as JSON by accessing the suunto://MDS/Logbook/{serial}>/ByID/{ID}/Data REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.  
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Number of the entry to get</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogbookDataJsonAsync(MdsConnectionContext, int) instead.")]
        Task<string> GetLogbookDataJsonAsync(string deviceName, int logId);

        /// <summary>
        /// Get data from a Logbook entry as JSON by accessing the suunto://MDS/Logbook/{serial}>/ByID/{ID}/Data REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.  
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="logId">Number of the entry to get</param>
        Task<string> GetLogbookDataJsonAsync(MdsConnectionContext connectionContext, int logId);

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="logId">Logbook entry to get</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogbookDataJsonAsync(MdsConnectionContext, int) instead.")]
        Task<BaseResult> GetLogbookDescriptorsAsync(string deviceName, int logId);

        /// <summary>
        /// Get Descriptors for a Logbook entry
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="logId">Logbook entry to get</param>
        Task<BaseResult> GetLogbookDescriptorsAsync(MdsConnectionContext connectionContext, int logId);

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://MDS/Logbook/{serial}>/Entries" REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogEntriesJsonAsync(MdsConnectionContext) instead.")]
        Task<LogEntriesMDSResult> GetLogEntriesJsonAsync(string deviceName);

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://MDS/Logbook/{serial}>/Entries" REST endpoint. 
        /// This MDS Logbook proxy service takes care of paging and also data-json conversion.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<LogEntriesMDSResult> GetLogEntriesJsonAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://{serial}/Mem/Logbook/Entries REST endpoint
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLogEntriesAsync(MdsConnectionContext) instead.")]
        Task<LogEntriesResult> GetLogEntriesAsync(string deviceName);

        /// <summary>
        /// Get details of Logbook entries by accessing the suunto://{serial}/Mem/Logbook/Entries REST endpoint
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<LogEntriesResult> GetLogEntriesAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetLoggerStatusAsync(MdsConnectionContext) instead.")]
        Task<LogStatusResult> GetLoggerStatusAsync(string deviceName);

        /// <summary>
        /// Get Logger status, CallAsync returns LogStatusResult object
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<LogStatusResult> GetLoggerStatusAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetMagInfoAsync(MdsConnectionContext) instead.")]
        Task<MagnInfo> GetMagInfoAsync(string deviceName);

        /// <summary>
        /// Get Magnetometer configuration
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<MagnInfo> GetMagInfoAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use GetTimeAsync(MdsConnectionContext) instead.")]
        Task<TimeResult> GetTimeAsync(string deviceName);

        /// <summary>
        /// Gets current time in number of microseconds since epoch 1.1.1970 (UTC).
        /// If not explicitly set, contains number of seconds since reset.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task<TimeResult> GetTimeAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetLedStateAsync(MdsConnectionContext...) instead.")]
        Task SetLedStateAsync(string deviceName, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red);

        /// <summary>
        /// Sets state of an LED
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="ledIndex">Index of the Led - use 0 for standard Movesense sensor</param>
        /// <param name="ledOn">Set on or off</param>
        /// <param name="ledColor">[optional]value from LedColor enumeration - default is LedColor.Red</param>
        Task SetLedStateAsync(MdsConnectionContext connectionContext, int ledIndex, bool ledOn, LedColor ledColor = LedColor.Red);

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetLoggerStatusAsync(MdsConnectionContext, bool) instead.")]
        Task SetLoggerStatusAsync(string deviceName, bool start);

        /// <summary>
        /// Set state of the Datalogger
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="start">Set true to start the datalogger, false to stop</param>
        Task SetLoggerStatusAsync(MdsConnectionContext connectionContext, bool start);

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetTimeAsync(MdsConnectionContext) instead.")]
        Task SetTimeAsync(string deviceName);

        /// <summary>
        /// Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC).
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        Task SetTimeAsync(MdsConnectionContext connectionContext);

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetupLoggerAsync(MdsConnectionContext, int) instead.")]
        Task SetupLoggerAsync(string deviceName, int freq = 26);

        /// <summary>
        /// Set configuration for the Datalogger - ONLY sets IMU9
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="freq">Sampling rate, e.g. 26 for 26Hz</param>
        Task SetupLoggerAsync(MdsConnectionContext connectionContext, int freq = 26);

        /// <summary>
        /// Set configuration for the Datalogger
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="dataLoggerConfig">Configuration to apply to the DataLogger. Config is an array of structs containing paths to the subscription of data to log.
        /// For example:             
        /// DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/" + freq) };
        /// DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
        /// </param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SetLoggerConfigAsync(MdsConnectionContext, DataLoggerConfig) instead.")]
        Task SetLoggerConfigAsync(string deviceName, DataLoggerConfig dataLoggerConfig);

        /// <summary>
        /// Set configuration for the Datalogger
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="dataLoggerConfig">Configuration to apply to the DataLogger. Config is an array of structs containing paths to the subscription of data to log.
        /// For example:             
        /// DataLoggerConfig.DataEntry[] entries = { new DataLoggerConfig.DataEntry("/Meas/IMU9/" + freq) };
        /// DataLoggerConfig config = new DataLoggerConfig(new DataLoggerConfig.Config(new DataLoggerConfig.DataEntries(entries)));
        /// </param>
        Task SetLoggerConfigAsync(MdsConnectionContext connectionContext, DataLoggerConfig dataLoggerConfig);

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeAccelerometerAsync(MdsConnectionContext...) instead.")]
        Task<IMdsSubscription> SubscribeAccelerometerAsync(string deviceName, Action<AccData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic linear acceleration measurements.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="notificationCallback">Callback function to receive the AccData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeAccelerometerAsync(MdsConnectionContext connectionContext, Action<AccData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeGyrometerAsync(MdsConnectionContext...) instead.")]
        Task<IMdsSubscription> SubscribeGyrometerAsync(string deviceName, Action<GyroData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Gyrometer data
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="notificationCallback">Callback function to receive the GyroData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeGyrometerAsync(MdsConnectionContext connectionContext, Action<GyroData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeIMU6Async(MdsConnectionContext...) instead.")]
        Task<IMdsSubscription> SubscribeIMU6Async(string deviceName, Action<IMU6Data> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic 6-axis IMU measurements (Acc + Gyro).
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="notificationCallback">Callback function to receive the IMU6Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeIMU6Async(MdsConnectionContext connectionContext, Action<IMU6Data> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeIMU9Async(MdsConnectionContext...) instead.")]
        Task<IMdsSubscription> SubscribeIMU9Async(string deviceName, Action<IMU9Data> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic 9-axis IMU measurements.
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="notificationCallback">Callback function to receive the IMU9Data</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeIMU9Async(MdsConnectionContext connectionContext, Action<IMU9Data> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeMagnetometerAsync(MdsConnectionContext...) instead.")]
        Task<IMdsSubscription> SubscribeMagnetometerAsync(string deviceName, Action<MagnData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to periodic Magnetometer data measurements
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="notificationCallback">Callback function to receive the MagnData</param>
        /// <param name="sampleRate">Sampling rate, e.g. 26 for 26Hz</param>
        Task<IMdsSubscription> SubscribeMagnetometerAsync(MdsConnectionContext connectionContext, Action<MagnData> notificationCallback, int sampleRate = 26);

        /// <summary>
        /// Subscribe to device time notifications
        /// </summary>
        /// <param name="deviceName">Name of the device, e.g. Movesense 174430000051</param>
        /// <param name="notificationCallback">Callback function to receive the time data</param>
        [Obsolete("Passing argument of deviceName is deprecated, please use SubscribeTimeAsync(MdsConnectionContext...) instead.")]
        Task<IMdsSubscription> SubscribeTimeAsync(string deviceName, Action<TimeNotificationResult> notificationCallback);

        /// <summary>
        /// Subscribe to device time notifications
        /// </summary>
        /// <param name="connectionContext">MdsConnectionContext for the device</param>
        /// <param name="notificationCallback">Callback function to receive the time data</param>
        Task<IMdsSubscription> SubscribeTimeAsync(MdsConnectionContext connectionContext, Action<TimeNotificationResult> notificationCallback);
    }
}
