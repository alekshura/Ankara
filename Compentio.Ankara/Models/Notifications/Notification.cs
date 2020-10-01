namespace Compentio.Ankara.Models.Notifications
{
    public class Notification
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
