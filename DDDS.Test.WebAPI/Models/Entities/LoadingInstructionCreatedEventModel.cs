namespace LGW.MessageDistributor.MessageBus.Domain.Models
{
    public class LoadingInstructionCreatedEventModel : EventModel
    {
        public int Id { get; set; }
        public int CityCode { get; set; }
        public string LoadingChannel { get; set; }
        public string RefNo { get; set; }
        public string MifareId { get; set; }
        public int CardRecNo { get; set; }
        public int RecType { get; set; }
        public int RecValue { get; set; }
    }
}