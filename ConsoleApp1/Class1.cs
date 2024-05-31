using ConsoleApp1;
using System.Text.Json;

public class MessageList
{
    public List<Message> Messages { get; set; }

    public MessageList()
    {
        Messages = new List<Message>();
    }

    public void AddMessage(Message message)
    {
        Messages.Add(message);
    }

    public void PrintMessages()
    {
        foreach (var message in Messages)
        {
            message.Print();
        }
    }

    public static MessageList DeserializeFromJson(string messageTextResponse)
    {
        return JsonSerializer.Deserialize<MessageList>(messageTextResponse);
    }
}