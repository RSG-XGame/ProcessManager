using PM.Connection;
using PM.Connection.Abstracts;
using PM.Connection.Commands.Requests;
using PM.Connection.Commands.Responses;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PM.ClientConnection
{
    public class ClientConnect : IDisposable
    {
        private object clientLocker;
        private CancellationTokenSource cancelRecivingMessageg;

        public delegate void CommandRecivedHandle(ClientConnect client, CommandBase command);
        public delegate void ClientConnectedHandle(ClientConnect client);
        public delegate void ClientDisconnectedHandle(ClientConnect client);

        public bool IsFirstMessage { get; private set; } = true;

        public Guid ClientId { get; set; } = Guid.NewGuid();
        public TcpClient Client { get; set; }
        public NetworkStream Stream
        {
            get
            {
                return Client?.GetStream();
            }
        }
        public string IPAddressServer { get; set; } = "127.0.0.1";
        public int PortServer { get; set; } = 1347;
        public string ClientName { get; set; } = "";

        public event CommandRecivedHandle CommandRecived;

        public event ClientConnectedHandle ClientConnected;
        public event ClientDisconnectedHandle ClientDisconnected;

        public bool IsConnected => Client?.Connected ?? false;


        public ClientConnect()
        {
            Client = new TcpClient();
            clientLocker = new object();
        }

        public void Connect()
        {
            if (disposedValue)
                return;

            lock (clientLocker)
            {
                try
                {
                    Client.Connect(IPAddressServer, PortServer);
                    Listen();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void Listen(bool waitFirstMessage = false)
        {
            IsFirstMessage = waitFirstMessage;
            ClientConnected?.Invoke(this);
            if (cancelRecivingMessageg == null || cancelRecivingMessageg.IsCancellationRequested)
            {
                cancelRecivingMessageg = new CancellationTokenSource();
                Task.Run((() => { RecivingCommands(cancelRecivingMessageg.Token); }));
            }
        }

        public void Disconnect()
        {
            if (disposedValue)
                return;

            lock (clientLocker)
            {
                if (cancelRecivingMessageg != null && !cancelRecivingMessageg.IsCancellationRequested)
                {
                    cancelRecivingMessageg.Cancel();
                }
                Client?.Close();
            }
            Thread.Sleep(1000);
        }

        private void RecivingCommands(CancellationToken token)
        {
            if (disposedValue)
                return;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (!Client.Connected)
                    {
                        Reconnect();
                    }
                    else
                    {
                        byte[] buffer = new byte[4];
                        int len = Stream.Read(buffer, 0, buffer.Length);
                        if (len > 0)
                        {
                            int bufferLength = BitConverter.ToInt32(buffer, 0);
                            buffer = new byte[bufferLength];
                            Stream.Read(buffer, 0, buffer.Length);

                            lock (clientLocker)
                            {
                                CommandBase command = buffer.DeserializeCommand();
                                CommandRecived?.Invoke(this, command);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientDisconnected?.Invoke(this);
                }
            }
        }

        public bool SendCommand(CommandRequest command)
        {
            bool result = false;
            if (disposedValue)
                return result;
            byte[] buffer = command.SerializeCommand();
            lock (clientLocker)
            {
                try
                {
                    if (!Client.Connected)
                    {
                        Reconnect();
                    }

                    byte[] metaData = BitConverter.GetBytes(buffer.Length);
                    byte[] resultBuffer = new byte[metaData.Length + buffer.Length];

                    Array.Copy(metaData, 0, resultBuffer, 0, metaData.Length);
                    Array.Copy(buffer, 0, resultBuffer, metaData.Length, buffer.Length);

                    Stream.Write(resultBuffer, 0, resultBuffer.Length);
                    result = true;
                }
                catch (Exception ex)
                {
                    //ConsoleOutput.WriteLineError($"Ошибка при отправки сообщения '{ex}'");
                }
            }
            return result;
        }

        private void Reconnect()
        {
            lock (clientLocker)
            {
                Client.Close();
                Client = new TcpClient();
                Client.Connect(IPAddressServer, PortServer);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disconnect();
                    Client.Dispose();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~ChatClient()
        // {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
