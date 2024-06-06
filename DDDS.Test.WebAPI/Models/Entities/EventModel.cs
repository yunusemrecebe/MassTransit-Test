namespace LGW.MessageDistributor.MessageBus.Domain.Models
{
    public class EventModel
    {
        public EventModel()
        {
            PublishDate = DateTime.Now;
        }

        public DateTime PublishDate { get; set; }
    }
}