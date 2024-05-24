namespace LibraryApplication.Models
{
    public class Message
    {
        public string User { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateTime { get; set; }
    }
}
