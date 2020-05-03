using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

// https://gist.github.com/darkguy2008/413a6fea3a5b4e67e5e0d96f750088a9
namespace CS.UDP.Sample01
{
  public class UDPSocket
  {
    private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private const int bufSize = 8 * 1024;
    private State state = new State();
    private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback recv = null;
    private string sReturnString = "";

    public class State
    {
        public byte[] buffer = new byte[bufSize];
    }

    public void Server(string address, int port)
    {
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
        Receive();            
    }

    public string Client(string address, int port)
    {
        _socket.Connect(IPAddress.Parse(address), port);
        return(Receive());
    }

    public void Send(string text)
    {
        byte[] data = Encoding.ASCII.GetBytes(text);
        _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = _socket.EndSend(ar);
            Console.WriteLine("SEND: {0}, {1}", bytes, text);
        }, state);
    }
  
    private string Receive()
    {
      sReturnString = "Before";
      _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
      {
          State so = (State)ar.AsyncState;
          int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
          _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
          Console.WriteLine("RECV: IP={0}; bytes={1}, return_val={2}\n", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));
          sReturnString = "Inside";
      }, state);
      Console.WriteLine("@ After stage:" + sReturnString + ">>>>>>");
      return (sReturnString);
    }
  }
}