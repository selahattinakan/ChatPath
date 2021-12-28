using System;

namespace ChatPath.Entity
{
    public class Message
    {
        public int MessageID { get; set; }
        public int ChannelID { get; set; }
        public string NickName { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }

    }
}
