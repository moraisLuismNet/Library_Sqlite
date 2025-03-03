namespace Library.DTOs
{
    public class BookDTO
    {
        public int Isbn { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public decimal Price { get; set; }
        public string? PhotoCover { get; set; }
        public bool Discontinued { get; set; }
        public string NameAuthor { get; set; }
        public string NamePublishingHouse { get; set; }
    }
}