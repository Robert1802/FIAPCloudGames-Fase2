namespace FIAPCloudGames.Domain.Entity
{
    public class LogEntity
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Properties { get; set; }
    }

}
