namespace Library.DTOs
{
    public class PublishingHouseBookAuthorDTO
    {
        public string Name { get; set; }
        public List<BookItemDTO> Books { get; set; }
        public List<AuthorDTO> Authors { get; set; }
    }
}
