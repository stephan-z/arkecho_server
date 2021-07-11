using ArkEcho.Core;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ArkEcho.Server
{
    public sealed class ArkEchoServer : IDisposable
    {
        public static ArkEchoServer Instance { get; } = new ArkEchoServer();

        public ServerConfig Config { get; private set; } = null;

        private MusicLibrary library = null;
        private MusicWorker musicWorker = null;

        private IWebHost host = null;

        private ArkEchoServer()
        {
            library = new MusicLibrary();
            musicWorker = new MusicWorker();
        }

        public bool Init(IWebHost Host)
        {
            if (Initialized)
                return Initialized;

            host = Host;

            Console.WriteLine("Initializing ArkEcho.Server");

            Config = new ServerConfig();
            if (!Config.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                Console.WriteLine("### No Config File found -> created new one, please configure. Stopping Server");
                return false;
            }
            else if (string.IsNullOrEmpty(Config.MusicFolder) || !Directory.Exists(Config.MusicFolder))
            {
                Console.WriteLine("### Music File Path not found! Enter Correct Path like: \"C:\\Users\\UserName\\Music\"");
                return false;
            }
            else
                Config.WriteOutputToConsole();

            musicWorker.Init(Config.MusicFolder);
            musicWorker.RunWorkerCompleted += MusicWorker_RunWorkerCompleted;

            musicWorker.RunWorkerAsync();

            Initialized = true;

            return Initialized;
        }

        private void MusicWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine($"Worker Completed!");
            if (e.Result != null)
                library = (MusicLibrary)e.Result;
            else
            {
                Console.WriteLine("### Error loading Music Library, stopping!");
                Stop();
            }
        }

        public List<MusicFile> GetAllMusicFiles()
        {
            return library.MusicFiles;
        }

        public List<AlbumArtist> GetAllAlbumArtists()
        {
            return library.AlbumArtists;
        }

        public List<Album> GetAllAlbum()
        {
            return library.Album;
        }

        public void Stop()
        {
            host.StopAsync();
        }

        public void Restart()
        {
            RestartRequested = true;
            Stop();
        }

        public bool Initialized { get; private set; } = false;

        public bool RestartRequested { get; private set; } = false;

        #region TCP Server

        //public ManualResetEvent allDone = new ManualResetEvent(false);

        //public void StartListening()
        //{
        //    // Establish the local endpoint for the socket.  
        //    // The DNS name of the computer  
        //    // running the listener is "host.contoso.com".  
        //    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //    IPAddress ipAddress = ipHostInfo.AddressList[0];
        //    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        //    // Create a TCP/IP socket.  
        //    Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //    // Bind the socket to the local endpoint and listen for incoming connections.  
        //    try
        //    {
        //        listener.Bind(localEndPoint);
        //        listener.Listen(100);

        //        while (!Stopping)
        //        {
        //            // Set the event to nonsignaled state.  
        //            allDone.Reset();

        //            // Start an asynchronous socket to listen for connections.  
        //            Console.WriteLine("Waiting for a connection...");
        //            listener.BeginAccept(new AsyncCallback(AcceptCallback),listener);

        //            // Wait until a connection is made before continuing.  
        //            allDone.WaitOne();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        //public void AcceptCallback(IAsyncResult ar)
        //{
        //    // Signal the main thread to continue.  
        //    allDone.Set();

        //    // Get the socket that handles the client request.  
        //    Socket listener = (Socket)ar.AsyncState;
        //    Socket handler = listener.EndAccept(ar);

        //    // Create the state object.  
        //    StateObject state = new StateObject();
        //    state.workSocket = handler;
        //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
        //        new AsyncCallback(ReadCallback), state);
        //}

        //public void ReadCallback(IAsyncResult ar)
        //{
        //    string content = string.Empty;

        //    // Retrieve the state object and the handler socket  
        //    // from the asynchronous state object.  
        //    StateObject state = (StateObject)ar.AsyncState;
        //    Socket handler = state.workSocket;

        //    // Read data from the client socket.
        //    int bytesRead = handler.EndReceive(ar);

        //    if (bytesRead > 0)
        //    {
        //        // There  might be more data, so store the data received so far.  
        //        state.sb.Append(Encoding.ASCII.GetString(
        //            state.buffer, 0, bytesRead));

        //        // Check for end-of-file tag. If it is not there, read
        //        // more data.  
        //        content = state.sb.ToString();
        //        if (content.IndexOf("<EOF>") > -1)
        //        {
        //            // All the data has been read from the
        //            // client. Display it on the console.  
        //            Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
        //                content.Length, content);
        //            // Echo the data back to the client.  
        //            Send(handler, content);
        //        }
        //        else
        //        {
        //            // Not all data received. Get more.  
        //            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
        //            new AsyncCallback(ReadCallback), state);
        //        }
        //    }
        //}

        //private void Send(Socket handler, String data)
        //{
        //    // Convert the string data to byte data using ASCII encoding.  
        //    byte[] byteData = Encoding.ASCII.GetBytes(data);

        //    // Begin sending the data to the remote device.  
        //    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        //}

        //private void SendCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Retrieve the socket from the state object.  
        //        Socket handler = (Socket)ar.AsyncState;

        //        // Complete sending the data to the remote device.  
        //        int bytesSent = handler.EndSend(ar);
        //        Console.WriteLine("Sent {0} bytes to client.", bytesSent);

        //        handler.Shutdown(SocketShutdown.Both);
        //        handler.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        //// State object for reading client data asynchronously  
        //public class StateObject
        //{
        //    // Client  socket.  
        //    public Socket workSocket = null;
        //    // Size of receive buffer.  
        //    public const int BufferSize = 1024;
        //    // Receive buffer.  
        //    public byte[] buffer = new byte[BufferSize];
        //    // Received data string.  
        //    public StringBuilder sb = new StringBuilder();
        //}

        #endregion

        private bool disposed;

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    musicWorker?.Dispose();
                    musicWorker = null;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
