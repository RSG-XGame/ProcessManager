using PM.ClientConnection;
using PM.Connection;
using PM.Connection.Abstracts;
using PM.Connection.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PM.ServerConnection
{
    public class ServerLestener : IDisposable
    {
        public delegate CommandResponse CommandProcessing(CommandRequest request);

        private TcpListener server;
        private List<ClientConnect> clients;
        private object clientLocker;
        private CancellationTokenSource cancelListen;
        private ManualResetEvent listenerStoped;
        private CommandProcessing processing;

        public string IPAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 1347;

        public ServerLestener()
        {
            clients = new List<ClientConnect>();
            clientLocker = new object();
            listenerStoped = new ManualResetEvent(false);
        }

        public void Initialization(CommandProcessing processing)
        {
            if (processing == null)
                throw new ArgumentNullException("Метод обработки не может быть пустым!");
            
            try
            {
                server = new TcpListener(System.Net.IPAddress.Parse(IPAddress), Port);
                this.processing = processing;
            }
            catch (Exception ex)
            {
                //ConsoleOutput.WriteLineError($"Ошибка инициализации сервера '{ex}'.");
            }
        }

        public void Listen()
        {
            if (cancelListen == null || cancelListen.IsCancellationRequested)
            {
                cancelListen = new CancellationTokenSource();
                server.Start();
                Task.Run((() => { DoWork(cancelListen.Token); }));
            }
        }
        public void Stop()
        {
            server.Stop();
            cancelListen?.Cancel();
            listenerStoped.WaitOne();
        }

        private void DoWork(CancellationToken token)
        {
            try
            {
                listenerStoped.Reset();
                while (!token.IsCancellationRequested)
                {
                    TcpClient client = server.AcceptTcpClient();

                    Task.Run((() =>
                    {
                        ClientConnect chatClient = new ClientConnect
                        {
                            Client = client
                        };
                        chatClient.CommandRecived += ChatClient_MessageRecived;
                        chatClient.ClientConnected += ChatClient_ClientConnected;
                        chatClient.ClientDisconnected += ChatClient_ClientDisconected;

                        chatClient.Listen(true);
                    }));
                }
            }
            catch (Exception ex)
            {
                //ConsoleOutput.WriteLineError($"Ошибка работы сервера: '{ex}'.");
            }
            finally
            {
                clients.ForEach(x => x.Dispose());
                clients.Clear();
                server?.Stop();
                listenerStoped.Set();
            }
        }

        private void ChatClient_ClientDisconected(ClientConnect client)
        {
            client.Disconnect();
            client.Dispose();
            lock (clientLocker)
            {
                clients.Remove(client);
            }
        }

        private void ChatClient_ClientConnected(ClientConnect client)
        {
            lock (clientLocker)
            {
                clients.Add(client);
            }
        }

        private void ChatClient_MessageRecived(ClientConnect client, CommandBase command)
        {
            //ConsoleOutput.WriteLineInfo($"Получено сообщение от '{client.ClientName}'. Сообщение '{message.CreateDate}': '{message.Message}'.");
            var response = processing(command as CommandRequest)?.SerializeCommand();
            try
            {
                if (response != null)
                {
                    byte[] metaData = BitConverter.GetBytes(response.Length);
                    byte[] result = new byte[metaData.Length + response.Length]; 
                    
                    Array.Copy(metaData, 0, result, 0, metaData.Length);
                    Array.Copy(response, 0, result, metaData.Length, response.Length);
                    client.Stream.Write(result, 0, result.Length);
                }
            }
            catch 
            {
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
                    cancelListen?.Cancel();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~ChatServer()
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
