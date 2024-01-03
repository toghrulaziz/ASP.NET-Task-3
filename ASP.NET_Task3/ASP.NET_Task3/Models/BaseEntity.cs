namespace ASP.NET_Task3.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime ModifiedTime { get; set; } = DateTime.Now;
    }
}
