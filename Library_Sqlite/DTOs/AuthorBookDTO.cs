namespace Library.DTOs
{
    public class AuthorBookDTO
    {
        public int IdAuthor { get; set; }
        public string NameAuthor { get; set; }
        public int TotalBooks { get; set; }
        public decimal AveragePrices { get; set; }
        public List<BookItemDTO> Books { get; set; }
    }
}