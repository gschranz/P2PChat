namespace P2PChat.ViewModels
{
    public class Message
    {
        public string Text
        {
            get; set;
        }

        public Message(string text)
        {
            this.Text = text;
        }
    }
}