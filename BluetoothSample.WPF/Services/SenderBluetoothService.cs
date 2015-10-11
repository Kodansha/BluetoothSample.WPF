﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bluetooth.Model;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace Bluetooth.Services
{
    /// <summary>
    /// Define the sender Bluetooth service.
    /// </summary>
    public sealed class SenderBluetoothService : ISenderBluetoothService
    {
         private readonly Guid _serviceClassId;
         private readonly BluetoothRadio myRadio;
         private readonly BluetoothEndPoint localEndpoint;
         private readonly BluetoothClient bluetoothClient;
         private readonly BluetoothComponent localComponent;
         private  string StatusDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="SenderBluetoothService"/> class. 
        /// </summary>
        public SenderBluetoothService()
        {
            // this guid is random, only need to match in Sender & Receiver
            // this is like a "key" for the connection!
            _serviceClassId = new Guid("e0cbf06c-cd8b-4647-bb8a-263b43f0f974");

            myRadio = BluetoothRadio.PrimaryRadio;
            if (myRadio == null)
            {
                Console.WriteLine("No radio hardware or unsupported software stack");
                //return;
            }
            RadioMode mode = myRadio.Mode;
            // Warning: LocalAddress is null if the radio is powered-off.
            Console.WriteLine("* Radio, address: {0:C}", myRadio.LocalAddress);

            // mac is mac address of local bluetooth device
             localEndpoint = new BluetoothEndPoint(myRadio.LocalAddress, BluetoothService.SerialPort);
            // client is used to manage connections
            bluetoothClient = new BluetoothClient(localEndpoint);
            // component is used to manage device discovery
            localComponent = new BluetoothComponent(bluetoothClient);
        }


        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <returns>The list of the devices.</returns>
        public async Task<IList<Device>> GetDevices()
        {
            // for not block the UI it will run in a different threat
            var task = Task.Run(() =>
            {
                var devices = new List<Device>();
                using (var bluetoothClient = new BluetoothClient())
                {
                    var array = bluetoothClient.DiscoverDevices();
                    var count = array.Length;
                    for (var i = 0; i < count; i++)
                    {
                        devices.Add(new Device(array[i]));
                    }
                }
                return devices;
            });
            return await task;
        }

        /// <summary>
        /// Sends the data to the Receiver.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="content">The content.</param>
        /// <returns>If was sent or not.</returns>
        public async Task<bool> Send(Device device, string content)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }

            // for not block the UI it will run in a different threat
            var task = Task.Run(() =>
            {
                using (var bluetoothClient = new BluetoothClient())
                {
                    try
                    {
                        BluetoothRadio myRadio = BluetoothRadio.PrimaryRadio;
                        if (myRadio == null)
                        {
                            Console.WriteLine("No radio hardware or unsupported software stack");
                            //return;
                        }
                        RadioMode mode = myRadio.Mode;
                        // Warning: LocalAddress is null if the radio is powered-off.
                        Console.WriteLine("* Radio, address: {0:C}", myRadio.LocalAddress);

                        // mac is mac address of local bluetooth device
                        BluetoothEndPoint localEndpoint = new BluetoothEndPoint(myRadio.LocalAddress, BluetoothService.SerialPort);
                        // client is used to manage connections
                        BluetoothClient localClient = new BluetoothClient(localEndpoint);
                        // component is used to manage device discovery
                        BluetoothComponent localComponent = new BluetoothComponent(localClient);

                        var ep = new BluetoothEndPoint(device.DeviceInfo.DeviceAddress, _serviceClassId);

                        // get stream for send the data
                        var bluetoothStream = bluetoothClient.GetStream();

                        // if all is ok to send
                        if (bluetoothClient.Connected && bluetoothStream != null)
                        {
                            // write the data in the stream
                            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                            bluetoothStream.Write(buffer, 0, buffer.Length);
                            bluetoothStream.Flush();
                            bluetoothStream.Close();
                            return true;
                        }
                        return false;
                    }
                    catch
                    {
                        // the error will be ignored and the send data will report as not sent
                        // for understood the type of the error, handle the exception
                    }
                }
                return false;
            });

            return await task;
        }

        /// <summary>
        /// Sends the data to the Receiver.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="content">The content.</param>
        /// <returns>If was sent or not.</returns>
        public async Task<bool> SendAsync(Device device, string content)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }

            // for not block the UI it will run in a different threat
            var task = Task.Run(() =>
            {
                try
                    {
                        //check if the device is connected
                        // get stream for send the data
                        if (bluetoothClient.Connected == true ){
                        var bluetoothStream = bluetoothClient.GetStream();

                        // if all is ok to send
                        if (bluetoothClient.Connected && bluetoothStream != null)
                        {
                            // write the data in the stream
                            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                            bluetoothStream.Write(buffer, 0, buffer.Length);
                            //bluetoothStream.Flush();
                            return true;     
                        }
                    }
                      }
                    catch
                    {
                        // the error will be ignored and the send data will report as not sent
                        // for understood the type of the error, handle the exception
                    }
                    return false;
                });

            return await task;
        }


        /// <summary>
        /// Async result for SendAsync.
        /// </summary>
        private void Connect(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                // client is connected now :)
                StatusDevice = "Connected";
            }
            else
            {
                StatusDevice = "NoConnected";
            }
        }

        /// <summary>
        /// Create a connection for the selected device.
        /// </summary>
        /// <param name="device">The device.</param>
        public void DeviceConnection(Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            var ep = new BluetoothEndPoint(device.DeviceInfo.DeviceAddress, _serviceClassId);
            // connecting
            if (bluetoothClient.Connected == false)
            {
                bluetoothClient.BeginConnect(device.DeviceInfo.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connect), device);
            }
        }

        /// <summary>
        /// Sends the data to the Receiver.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="content">The content.</param>
        /// <returns>If was sent or not.</returns>
        //public async Task<bool> CheckConnection()
        public bool CheckConnection()
        {
            if (StatusDevice == "Connected")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
    
}
}
