namespace Library.DTOs
{
    public class AuthorBookDTO
    {
        public int IdAuthor { get; set; }
        public string Name { get; set; }
        public int TotalBooks { get; set; }
        public decimal AveragePrices { get; set; }
        public List<BookItemDTO> Books { get; set; }
    }
}