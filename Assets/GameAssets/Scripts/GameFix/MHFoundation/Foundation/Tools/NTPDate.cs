using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Foundation
{
    /// <summary>
    /// Class implements simplified NTP protocol. Based on RFC 5905 (https://www.ietf.org/rfc/rfc5905.txt)
    /// </summary>
    public class NTPDate : MonoSingleton<NTPDate>
    {
        /// <summary>
        /// Address to server supporting NTP (prefered 'pool.ntp.org' which will automatically redirect to best one).
        /// </summary>
        private const string NTP_SERVER = "pool.ntp.org";
        /// <summary>
        /// NTP port on server. In most cases is 123.
        /// </summary>
        private const int NTP_SERVER_PORT = 123;
        /// <summary>
        /// Buffer size transfered between client <=> server.
        /// </summary>
        private const int NTP_BUFFER_SIZE = 48;
        /// <summary>
        /// Buffer header used for protocol configuration (Eg.: 0x23: 00...(0 - no warnings)..100..(4 - protocol version)..011(3 - client mode)).
        /// </summary>
        private const int NTP_HEADER = 0x23;
        /// <summary>
        /// Request timeout.
        /// </summary>
        private const float NTP_TIMEOUT = 3f;

        public delegate void TimeInitialized();
        /// <summary>
        /// Dispached when time is successfuly recived from server.
        /// </summary>
        public event TimeInitialized OnTimeInitialized;

        public delegate void TimeRequestError();
        /// <summary>
        /// Dispached when cannot recive time from server.
        /// </summary>
        public event TimeRequestError OnTimeRequestError;

        /// <summary>
        /// Date recived from server or machine UTC time.
        /// </summary>
        public DateTime Date
        {
            get
            {
                lastSytemUptime = GetSystemUptime();
                // Add elapsed seconds from last time update
                return ntpDate.AddSeconds(lastSytemUptime - updateTime);
            }
        }

        private bool isTimeInitialized;
        public bool IsTimeInitialized
        {
            get
            {
                return isTimeInitialized;
            }
            private set
            {
                if (isTimeInitialized != value)
                {
                    isTimeInitialized = value;

                    if (IsTimeInitialized && OnTimeInitialized != null)
                        OnTimeInitialized();
                }
            }
        }

        /// <summary>
        /// Date from server on DateTime.UtcNow.
        /// </summary>
        private DateTime ntpDate = DateTime.UtcNow;
        private DateTime initialDate = new System.DateTime(1970, 1, 1);
        /// <summary>
        /// Last update time in seconds sice game started.
        /// </summary>
        private float updateTime;
        /// <summary>
        /// Stored last system uptime.
        /// </summary>
        private float lastSytemUptime;

        private UdpClient udpClient;
        private Thread timeRequestThread;
        private volatile bool threadRunning = false;
        private byte[] recivedTimeData;

        protected override void Awake()
        {
            if (!threadRunning)
                RefreshNetworkTimeAsync();
        }

        private void OnApplicationQuit()
        {
            Dispose();
        }

        private void OnApplicationPause(bool isPaused)
        {
            // If device is restarted system uptime will reset
            if (IsTimeInitialized && !isPaused && GetSystemUptime() < lastSytemUptime && !threadRunning)
                RefreshNetworkTimeAsync();
        }

        private void Request()
        {
            // Get potentialy free port and machine adress
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // Clear buffer to know if time is successfuly updated
            recivedTimeData = null;

            try // Sockets sometimes stays open even if not used. In that case we need to catch Exeption.
            {
                udpClient = new UdpClient(ipEndPoint);
            }
            catch (Exception e)
            {
                Debug.LogWarning(string.Format("Cannot open UDP port: {0}", e.Message));
                threadRunning = false;
                return;
            }

            byte[] sendData = new byte[NTP_BUFFER_SIZE];
            sendData[0] = NTP_HEADER;

            try // If host is unreachable we need to catch exeption.
            {
                udpClient.Send(sendData, sendData.Length, NTP_SERVER, NTP_SERVER_PORT);
                recivedTimeData = udpClient.Receive(ref ipEndPoint);
            }
            catch (Exception e)
            {
                Debug.LogWarning(string.Format("Cannot get time via NTP: {0}", e.Message));
            }

            udpClient.Close();

            threadRunning = false;
        }

        private IEnumerator WaitForRequest()
        {
            var startWaitTime = Time.realtimeSinceStartup;
            while (threadRunning && Time.realtimeSinceStartup - startWaitTime < NTP_TIMEOUT)
            {
                yield return null;
            }

            if (threadRunning || recivedTimeData == null || recivedTimeData.Length == 0)
            {
                // If cannot get time from server, use device time.
                SetTime(DateTime.UtcNow);

                Debug.LogWarning("NTP is using device time.");

                if (OnTimeRequestError != null)
                    OnTimeRequestError();
            }
            else
            {
                DateTime date = new DateTime(1900, 1, 1);
                // NTP server returns seconds since 01.01.1900
                var high = (double)BitConverter.ToUInt32(new byte[] { recivedTimeData[43], recivedTimeData[42], recivedTimeData[41], recivedTimeData[40] }, 0);
                var low = (double)BitConverter.ToUInt32(new byte[] { recivedTimeData[47], recivedTimeData[46], recivedTimeData[45], recivedTimeData[44] }, 0);
                date = date.AddSeconds(high + low / UInt32.MaxValue);
                if (date > initialDate)
                    SetTime(date);
            }
        }

        /// <summary>
        /// Clear data and get time from server
        /// </summary>
        public void Dispose()
        {
            if (udpClient != null)
                udpClient.Close();
        }

        /// <summary>
        /// Recive time from server or sets machine time as current if fail
        /// </summary>
        public void RefreshNetworkTimeAsync()
        {
            threadRunning = true;
            StartCoroutine(WaitForRequest());

            timeRequestThread = new Thread(new ThreadStart(Request));
            timeRequestThread.Start();
        }

        /// <summary>
        /// Manualy set current time
        /// </summary>
        public void SetTime(DateTime time)
        {
            updateTime = GetSystemUptime();
            ntpDate = time;
            IsTimeInitialized = true;
        }

        /// <summary>
        /// Clear data and get time from server
        /// </summary>
        public void Reset()
        {
            IsTimeInitialized = false;

            Dispose();
            RefreshNetworkTimeAsync();
        }

        /// <summary>
        /// Returns system uptime in seconds. 
        /// Notice that on iOS Time.realtimeSinceStartup is not correct after device suspension.
        /// In that case is necessary to get e.g. kernel uptime instead.
        /// </summary>
        private float GetSystemUptime()
        {
            return Time.realtimeSinceStartup;
        }
    }
}
