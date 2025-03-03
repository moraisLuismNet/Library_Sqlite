namespace Library.DTOs
{
    public class PublishingHouseBookAuthorDTO
    {
        public string NamePublishingHouse { get; set; }
        public List<BookItemDTO> Books { get; set; }
        public List<AuthorDTO> Authors { get; set; }
    }
}
