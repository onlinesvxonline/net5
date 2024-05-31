using ConsoleApp1;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class ClientSendCommand : ICommand
    {
        private UdpClient udpClient;
        private IPEndPoint iPEndPoint;
        private string? messageText;

        public ClientSendCommand(UdpClient udpClient, IPEndPoint iPEndPoint, string? messageText)
        {
            this.udpClient = udpClient;
            this.iPEndPoint = iPEndPoint;
            this.messageText = messageText;
        }

        public void Execute()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(messageText);
            udpClient.Send(buffer, buffer.Length, iPEndPoint);
        }
    }
}