﻿using Android.Util;
using MdsLibrary;
using MovesenseDemo.Model;
using Newtonsoft.Json.Linq;
using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovesenseDemo
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        IDisposable scan;
        public IAdapter BleAdapter => CrossBleAdapter.Current;

        private void OnClicked(object sender, EventArgs e)
        {
            if (BleAdapter.Status == AdapterStatus.PoweredOn)
            {
                DoScan();
            }
            else
            {
                BleAdapter.WhenStatusChanged().Subscribe(status =>
                {
                    if (status == AdapterStatus.PoweredOn)
                    {
                        DoScan();
                    }
                });
            }
        }

        private void DoScan()
        {
            StatusLabel.Text = "Scanning for devices...";
            scan = this.BleAdapter.Scan()
            .Subscribe(this.OnScanResult);
        }

        public void StopScanning()
        {
            this.scan?.Dispose();
        }

        async void OnScanResult(IScanResult result)
        {
            // Only interested in Movesense devices
            if (result.Device.Name != null)
            {
                if (result.Device.Name.StartsWith("Movesense"))
                {
                    StopScanning();

                    var movesense = Plugin.Movesense.CrossMovesense.Current;

                    movesense.ConnectionListener.DeviceDisconnected += async (s, a) =>
                        {
                            await DisplayAlert("Disconnection", $"Device {a.Serial} disconnected", "OK");
                        };

                    // Now do the Mds connection
                    var sensor = result.Device;
                    StatusLabel.Text = $"Connecting to device {sensor.Name}";
					DeviceName.Text = $"{sensor.Name}";
                    var movesenseDevice = await movesense.ConnectMdsAsync(sensor.Uuid);

                    // Talk to the device
                    StatusLabel.Text = "Getting device info";
					var info = await movesenseDevice.GetDeviceInfoAsync();
					DeviceFirmware.Text = $"Firmware Version: {info.DeviceInfo.Sw}";

					StatusLabel.Text = "Getting battery level";
                    var batt = await movesenseDevice.GetBatteryLevelAsync();
					DeviceBattery.Text = $"Battery Level: {batt.ChargePercent}";

					StatusLabel.Text = "Getting Heart Rate";
					var hrData = await movesense.ApiSubscriptionAsync<string>(movesenseDevice: movesenseDevice, $"/Meas/HR",
						(hr) =>
						{
							TestData(hr);
						});

					// Turn on the LED
					//StatusLabel.Text = "Turning on LED";
					//await movesenseDevice.SetLedStateAsync(0, true);

					//await DisplayAlert(
					//    "Success", 
					//    $"Communicated with device {sensor.Name}, firmware version is: {info.DeviceInfo.Sw}, battery: {batt.ChargePercent}", 
					//    "OK");

					// Turn the LED off again
					//StatusLabel.Text = "Turning off LED";
					//await movesenseDevice.SetLedStateAsync(0, false);

					// Disconnect Mds
					//StatusLabel.Text = "Disconnecting";
					//await movesenseDevice.DisconnectMdsAsync();
					//StatusLabel.Text = "Disconnected";

				}
            }
        }

		private void TestData(string hr) {
			JObject data = JObject.Parse(hr);
			DeviceHR.Text = $"Current Heart Rate: {string.Format("{0:.##}", data["Body"]["average"])}";
		}
	}
}
