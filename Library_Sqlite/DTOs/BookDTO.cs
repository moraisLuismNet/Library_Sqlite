namespace Library.DTOs
{
    public class BookDTO
    {
        public int IdBook { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public decimal Price { get; set; }
        public string? PhotoCover { get; set; }
        public bool Discontinued { get; set; }
        public int AuthorId { get; set; }
        public int PublishingHouseId { get; set; }
    }
}