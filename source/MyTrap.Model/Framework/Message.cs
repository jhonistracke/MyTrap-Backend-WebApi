namespace MyTrap.Model.Framework
{
    public class Message
    {
        public string Content { get; set; }

        public Message(string message)
        {
            Content = message;
        }
    }
}