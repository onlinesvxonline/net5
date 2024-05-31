using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    public interface ICommand
    {
        void Execute();
    }

    public class ServerReceiveCommand : ICommand
    {
        private UdpClient udpClient;
        private IPEndPoint iPEndPoint;
        private string messageText;
        private MessageList messageList;

        public ServerReceiveCommand(UdpClient udpClient, IPEndPoint iPEndPoint, string messageText, MessageList messageList)
        {
            this.udpClient = udpClient;
            this.iPEndPoint = iPEndPoint;
            this.messageText = messageText;
            this.messageList = messageList;
        }

        public void Execute()
        {
            if (messageText.ToLower() == "exit")
            {
                Console.WriteLine("Сервер завершает работу...");
            }
            else
            {
                Message message = Message.DeserializeFromJson(messageText);
                messageList.AddMessage(message);
                messageList.PrintMessages();

                byte[] confirmationData = Encoding.UTF8.GetBytes("Сообщение успешно получено");
                udpClient.Send(confirmationData, confirmationData.Length, iPEndPoint);
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Server("Server", cancellationToken);

            Console.WriteLine("Нажмите любую клавишу для завершения работы сервера...");
            Console.ReadKey();

            cancellationTokenSource.Cancel();
        }

        public static void Server(string name, CancellationToken cancellationToken)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            MessageList messageList = new MessageList();

            Console.WriteLine("Сервер ждет сообщение от клиента");

            while (!cancellationToken.IsCancellationRequested)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                string messageText = Encoding.UTF8.GetString(buffer);

                ICommand command = new ServerReceiveCommand(udpClient, iPEndPoint, messageText, messageList);
                ThreadPool.QueueUserWorkItem(obj => { command.Execute(); });
            }

            udpClient.Close();
        }
    }
}