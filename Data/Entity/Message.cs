namespace Data.Entity
{
    public class Message
    {
        public string User { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateTime { get; set; }
        public string ConnectionId { get; set; }
    }
}
