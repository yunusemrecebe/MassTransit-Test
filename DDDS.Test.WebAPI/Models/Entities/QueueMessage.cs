using DDDS.Test.WebAPI.Models.Interface;

namespace DDDS.Test.WebAPI.Models.Entities
{
    public class QueueMessage : IQueueMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
