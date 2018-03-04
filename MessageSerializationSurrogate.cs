using System.Runtime.Serialization;

namespace P2PChat.ViewModels
{
    internal class MessageSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Text", ((Message)obj).Text);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Message msg = (Message)obj;
            msg.Text = info.GetString("Text");
            return msg;
        }
    }
}