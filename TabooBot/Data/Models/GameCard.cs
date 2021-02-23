namespace TabooBot.Data.Models
{
    public sealed class GameCard
    {
        public int Id { get; set; }
        // for downloading
        public string FileId { get; set; }
        // for checking dublicates
        public string UniqueFileId { get; set; }

    }
}