namespace kafka.ProductApi.Models
{
    public class IEventHandler
    {
        public Guid EventID { get; set; }
        public string EventName { get; set; }
        public string TraceId { get; set; }
    }
}
