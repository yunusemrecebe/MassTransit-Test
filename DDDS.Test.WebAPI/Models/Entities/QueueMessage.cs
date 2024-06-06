using DDDS.Test.WebAPI.Models.Interface;
using LGW.MessageDistributor.MessageBus.Domain.Models;

namespace DDDS.Test.WebAPI.Models.Entities
{
    public class QueueMessage : EventModel, IQueueMessage
    {
        public int Id { get; set; }
        public int CityCode { get; set; }
        public string LoadingChannel { get; set; }
        public string RefNo { get; set; }
        public string MifareId { get; set; }
        public int CardRecNo { get; set; }
        public int RecType { get; set; }
        public int RecValue { get; set; }
        DateTime IEventModel.PublishDate { get => base.PublishDate ; set => throw new NotImplementedException(); }
    }
}
