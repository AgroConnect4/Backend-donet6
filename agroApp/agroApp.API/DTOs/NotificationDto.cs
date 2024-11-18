namespace agroApp.API.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}