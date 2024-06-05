using DDDS.Test.WebAPI.Models.Interface;

namespace DDDS.Test.WebAPI.Models.Entities
{
    public class ThresholdExceededMessage : IThresholdExceededMessage
    {
        public int CityCode { get; set; }
    }
}
