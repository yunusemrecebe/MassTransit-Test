namespace DDDS.Test.WebAPI.Models.Interface
{
    public interface IQueueMessage
    {
        public int Id { get; set; }
        public int CityCode { get; set; }
        public string Name { get; set; }
    }
}
