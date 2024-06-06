namespace LGW.MessageDistributor.MessageBus.Domain.Models
{
    public class LoadingInstructionThresholdExceededEventModel : EventModel
    {
        public int CityCode { get; set; }
    }
}