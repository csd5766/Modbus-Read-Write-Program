using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection.Emit;
using System.Threading;


/// <summary>
/// Modbus TCP common driver class. 
/// </summary>
namespace ModbusTCP
{
    /// <summary>
    /// Modbus TCP common driver class. 
    /// </summary>
    /// 
    /// This class implements a modbus TCP master driver. It supports the following commands:
    /// 
    /// Read coils
    /// Read discrete inputs
    /// Write single coil
    /// Write multiple cooils
    /// Read holding register
    /// Read input register
    /// Write single register
    /// Write multiple register
    /// 
    /// All commands can be sent in synchronous or asynchronous mode. If a value is accessed
    /// in synchronous mode the program will stop and wait for slave to response. If the 
    /// slave didn't answer within a specified time a timeout exception is called.
    /// The class uses multi threading for both synchronous and asynchronous access. For
    /// the communication two lines are created. This is necessary because the synchronous
    /// thread has to wait for a previous command to finish.
    /// The synchronous channel can be disabled during connection. This can be necessary when
    /// the slave only supports one connection.
    /// 
    public class Master
    {
        // ------------------------------------------------------------------------
        // Constants for access
        private const byte fctReadCoil = 1;
        private const byte fctReadDiscreteInputs = 2;
        private const byte fctReadHoldingRegister = 3;
        private const byte fctReadInputRegister = 4;
        private const byte fctWriteSingleCoil = 5;
        private const byte fctWriteSingleRegister = 6;
        private const byte fctWriteMultipleCoils = 15;
        private const byte fctWriteMultipleRegister = 16;
        private const byte fctReadWriteMultipleRegister = 23;
       

        

        /// <summary>Constant for exception illegal function.</summary>
        public const byte excIllegalFunction = 1;
        /// <summary>Constant for exception illegal data address.</summary>
        public const byte excIllegalDataAdr = 2;
        /// <summary>Constant for exception illegal data value.</summary>
        public const byte excIllegalDataVal = 3;
        /// <summary>Constant for exception slave device failure.</summary>
        public const byte excSlaveDeviceFailure = 4;
        /// <summary>Constant for exception acknowledge. This is triggered if a write request is executed while the watchdog has expired.</summary>
        public const byte excAck = 5;
        /// <summary>Constant for exception slave is busy/booting up.</summary>
        public const byte excSlaveIsBusy = 6;
        /// <summary>Constant for exception gate path unavailable.</summary>
        public const byte excGatePathUnavailable = 10;
        /// <summary>Constant for exception not connected.</summary>
        public const byte excExceptionNotConnected = 253;
        /// <summary>Constant for exception connection lost.</summary>
        public const byte excExceptionConnectionLost = 254;
        /// <summary>Constant for exception response timeout.</summary>
        public const byte excExceptionTimeout = 255;
        /// <summary>Constant for exception wrong offset.</summary>
        private const byte excExceptionOffset = 128;
        /// <summary>Constant for exception send failt.</summary>
        private const byte excSendFailt = 100;

        // ------------------------------------------------------------------------
        // Private declarations
        private static ushort _timeout = 100;
        private static ushort _refresh = 10;
        private static bool _connected = false;
        private static bool _no_sync_connection = false;

        private Socket tcpAsyCl;
        private byte[] tcpAsyClBuffer = new byte[2048];

        private Socket tcpSynCl;
        private byte[] tcpSynClBuffer = new byte[2048];

        public int conCount = 0;
        public int conflag = 0;

        // ------------------------------------------------------------------------
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public delegate void ResponseData(ushort id, byte unit, byte function, byte[] data);
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public event ResponseData OnResponseData;
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public delegate void ExceptionData(ushort id, byte unit, byte function, byte exception);
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public event ExceptionData OnException;

        /// <summary>Create master instance with parameters.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        /// <param name="no_sync_connection">Disable second connection for synchronous requests</param>
        public Master(string ip, ushort port, bool no_sync_connection)
        {
             bool a = Connect(ip, port, no_sync_connection);
             
        }

        // ------------------------------------------------------------------------
        /// <summary>Start connection to slave.</summary>
        /// <param name="ip">IP adress of modbus slave.</param>
        /// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
        /// <param name="no_sync_connection">Disable sencond connection for synchronous requests</param>
        public bool Connect(string ip, ushort port, bool no_sync_connection)
        {
            try
            {
                IPAddress _ip;
                _no_sync_connection = no_sync_connection;
                if (IPAddress.TryParse(ip, out _ip) == false)
                {
                    IPHostEntry hst = Dns.GetHostEntry(ip);
                    ip = hst.AddressList[0].ToString();
                }
                // ----------------------------------------------------------------
                // Connect asynchronous client
                tcpAsyCl = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult asyncResult =  tcpAsyCl.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port),null,null);
                if (asyncResult.AsyncWaitHandle.WaitOne(1000, false))
                {
                    tcpAsyCl.EndConnect(asyncResult);
                    conflag = 1;
                }
                else
                {
                    //throw new Exception(string.Format("{0}:{1}에 연결할111 수 없습니다(Timeout).", IPAddress.Parse(ip), port));
                }
                tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _timeout);
                tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _timeout);
                tcpAsyCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);

              
                // ----------------------------------------------------------------
                // Connect synchronous client
                //if (!_no_sync_connection)
                //{
                //    tcpSynCl = new Socket(IPAddress.Parse(ip).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //    tcpSynCl.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                //    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, _timeout);
                //    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, _timeout);
                //    tcpSynCl.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);
                //}
                //_connected = true;
            }
            catch (System.IO.IOException error)
            {
                _connected = false;
                throw (error);
            }
            if (conflag == 1) return true;
            else return false;
        }

        public void Dispose()
        {
            if (tcpAsyCl != null)
            {
                if (tcpAsyCl.Connected)
                {
                    //try { tcpAsyCl.Shutdown(SocketShutdown.Both); }
                    //catch { }
                    tcpAsyCl.Close();
                    conflag = 0;
                }
                tcpAsyCl = null;
            }
            #region 동기 종료 
            //if (tcpSynCl != null)
            //{
            //    if (tcpSynCl.Connected)
            //    {
            //        try { tcpSynCl.Shutdown(SocketShutdown.Both); }
            //        catch { }
            //        tcpSynCl.Close();
            //    }
            //    tcpSynCl = null;
            //}
            #endregion
        }

        internal void CallException(ushort id, byte unit, byte function, byte exception)
        {
            if ((tcpAsyCl == null) || (tcpSynCl == null && !_no_sync_connection)) return;
            if (exception == excExceptionConnectionLost)
            {
                tcpSynCl = null;
                tcpAsyCl = null;
            }
            if(OnException != null) OnException(id, unit, function, exception);
        }

        internal static UInt16 SwapUInt16(UInt16 inValue)
        {
            return (UInt16)(((inValue & 0xff00) >> 8) |
                     ((inValue & 0x00ff) << 8));
        }
        // ------------------------------------------------------------------------
        /// <summary>Read holding registers from slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address from where the data read begins.</param>
        /// <param name="numInputs">Length of data.</param>
        public void ReadHoldingRegister(ushort id, byte unit, ushort startAddress, ushort numInputs)
        {
            if (numInputs > 125)
            {
                CallException(id, unit, fctReadHoldingRegister, excIllegalDataVal);
                return;
            }
            //Console.WriteLine(id +"-"+ unit + "-" + startAddress + "-" + numInputs + "-" + fctReadHoldingRegister + "-" + id);

            WriteAsyncData(CreateReadHeader(id, unit, startAddress, numInputs, fctReadHoldingRegister), id);
        }
        // ------------------------------------------------------------------------
        /// <summary>Write multiple registers in slave asynchronous. The result is given in the response function.</summary>
        /// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
        /// <param name="unit">Unit identifier (previously slave address). In asynchonous mode this unit is given to the callback function.</param>
        /// <param name="startAddress">Address to where the data is written.</param>
        /// <param name="values">Contains the register information.</param>
        public void WriteMultipleRegister(ushort id, byte unit, ushort startAddress, byte[] values)
        {
            ushort numBytes = Convert.ToUInt16(values.Length);
            if (numBytes > 250)
            {
                CallException(id, unit, fctWriteMultipleRegister, excIllegalDataVal);
                return;
            }

            if (numBytes % 2 > 0) numBytes++;
            byte[] data;
            byte[] data2;
            data = CreateWriteHeader(id, unit, startAddress, Convert.ToUInt16(numBytes / 2), Convert.ToUInt16(numBytes + 2), fctWriteMultipleRegister);
            data2 = CreateWriteHeader(id, unit, 718, Convert.ToUInt16(numBytes / 2), Convert.ToUInt16(numBytes + 2), fctWriteMultipleRegister);
            Array.Copy(values, 0, data, 13, values.Length);
            WriteAsyncData(data, id);
        }


        #region WriteSingleRegister
        public void WriteSingleRegister(ushort id, byte unit, ushort startAddress, byte[] values)
        {
            if (values.GetUpperBound(0) != 1)
            {
                CallException(id, unit, fctReadCoil, excIllegalDataVal);
                return;
            }
            byte[] data;
            data = CreateWriteHeader(id, unit, startAddress, 1, 1, fctWriteSingleRegister);
            data[10] = values[0];
            data[11] = values[1];
            WriteAsyncData(data, id);
        }
        #endregion
        // ------------------------------------------------------------------------
        // Create modbus header for read action
        private byte[] CreateReadHeader(ushort id, byte unit, ushort startAddress, ushort length, byte function)
        {
            byte[] data = new byte[12];

            byte[] _id = BitConverter.GetBytes((short)id);
            data[0] = _id[1];			    // Slave id high byte
            data[1] = _id[0];				// Slave id low byte
            data[5] = 6;					// Message size
            data[6] = unit;					// Slave address
            data[7] = function;				// Function code
            byte[] _adr = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startAddress));
            data[8] = _adr[0];				// Start address
            data[9] = _adr[1];				// Start address
            byte[] _length = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)length));
            data[10] = _length[0];			// Number of data to read
            data[11] = _length[1];			// Number of data to read
           /* for(int i =0; i < 12; i++)
            {
                Console.WriteLine(IPAddress.HostToNetworkOrder((short)startAddress));
            }*/
            
            return data;
        }

        // ------------------------------------------------------------------------
        // Create modbus header for write action
        private byte[] CreateWriteHeader(ushort id, byte unit, ushort startAddress, ushort numData, ushort numBytes, byte function)
        {
            byte[] data = new byte[numBytes + 11];

            byte[] _id = BitConverter.GetBytes((short)id);
            data[0] = _id[1];				// Slave id high byte
            data[1] = _id[0];				// Slave id low byte
            byte[] _size = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)(5 + numBytes)));
            data[4] = _size[0];				// Complete message size in bytes
            data[5] = _size[1];				// Complete message size in bytes
            data[6] = unit;					// Slave address
            data[7] = function;				// Function code
            byte[] _adr = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)startAddress));
            data[8] = _adr[0];				// Start address
            data[9] = _adr[1];				// Start address
            if (function >= fctWriteMultipleCoils)
            {
                byte[] _cnt = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)numData));
                data[10] = _cnt[0];			// Number of bytes
                data[11] = _cnt[1];			// Number of bytes
                data[12] = (byte)(numBytes - 2);
            }
            return data;
        }

        // ------------------------------------------------------------------------
        // Write asynchronous data
        private void WriteAsyncData(byte[] write_data, ushort id)
        {
            if ((tcpAsyCl != null) && (tcpAsyCl.Connected))
            {
                try
                {
                        tcpAsyCl.BeginSend(write_data, 0, write_data.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                }
                catch (SystemException)
                {
                    CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
                }
            }
            else CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
        }

        // ------------------------------------------------------------------------
        // Write asynchronous data acknowledge
        private void OnSend(System.IAsyncResult result)
        {
            //Int32 size = tcpAsyCl.EndSend(result);

            if (result.IsCompleted == false) CallException(0xFFFF, 0xFF, 0xFF, excSendFailt);

            else
            {
                try {
                    tcpAsyCl.BeginReceive(tcpAsyClBuffer, 0, tcpAsyClBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), tcpAsyCl);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                    
            }
        }

        // ------------------------------------------------------------------------
        // Write asynchronous data response
        private void OnReceive(System.IAsyncResult result)
        {
            if (tcpAsyCl == null) return;

            try
            {
                //tcpAsyCl.EndReceive(result);
                if (result.IsCompleted == false) CallException(0xFF, 0xFF, 0xFF, excExceptionConnectionLost);
            }
            catch (Exception) { }

            ushort id = SwapUInt16(BitConverter.ToUInt16(tcpAsyClBuffer, 0));
            byte unit = tcpAsyClBuffer[6];
            byte function = tcpAsyClBuffer[7];
            byte[] data;

            // ------------------------------------------------------------
            // Write response data
            if ((function >= fctWriteSingleCoil) && (function != fctReadWriteMultipleRegister))
            {
                data = new byte[2];
                Array.Copy(tcpAsyClBuffer, 10, data, 0, 2);
           
            }
            // ------------------------------------------------------------
            // Read response data
            else
            {
                    data = new byte[tcpAsyClBuffer[8]];
                    Array.Copy(tcpAsyClBuffer, 9, data, 0, tcpAsyClBuffer[8]);
                    //conCount++;
            }
            // ------------------------------------------------------------
            // Response data is slave exception
            if (function > excExceptionOffset)
            {
                function -= excExceptionOffset;
                CallException(id, unit, function, tcpAsyClBuffer[8]);
            }
            // ------------------------------------------------------------
            // Response data is regular data
            else if (OnResponseData != null)
            {
                    OnResponseData(id, unit, function, data);   
            }
        }


        #region 동기 방식 연결
        // ------------------------------------------------------------------------
        // Write data and and wait for response
        //private byte[] WriteSyncData(byte[] write_data, ushort id)
        //{

        //    if (tcpSynCl.Connected)
        //    {
        //        try
        //        {
        //            tcpSynCl.Send(write_data, 0, write_data.Length, SocketFlags.None);
        //            int result = tcpSynCl.Receive(tcpSynClBuffer, 0, tcpSynClBuffer.Length, SocketFlags.None);

        //            byte unit = tcpSynClBuffer[6];
        //            byte function = tcpSynClBuffer[7];
        //            byte[] data;

        //            if (result == 0) CallException(id, unit, write_data[7], excExceptionConnectionLost);

        //            // ------------------------------------------------------------
        //            // Response data is slave exception
        //            if (function > excExceptionOffset)
        //            {
        //                function -= excExceptionOffset;
        //                CallException(id, unit, function, tcpSynClBuffer[8]);
        //                return null;
        //            }
        //            // ------------------------------------------------------------
        //            // Write response data
        //            else if ((function >= fctWriteSingleCoil) && (function != fctReadWriteMultipleRegister))
        //            {
        //                data = new byte[2];
        //                Array.Copy(tcpSynClBuffer, 10, data, 0, 2);
        //            }
        //            // ------------------------------------------------------------
        //            // Read response data
        //            else
        //            {
        //                data = new byte[tcpSynClBuffer[8]];
        //                Array.Copy(tcpSynClBuffer, 9, data, 0, tcpSynClBuffer[8]);
        //            }
        //            return data;
        //        }
        //        catch (SystemException)
        //        {
        //            CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
        //        }
        //    }
        //    else CallException(id, write_data[6], write_data[7], excExceptionConnectionLost);
        //    return null;
        //}
    }
}
#endregion