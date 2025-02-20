namespace Library.DTOs
{
    public class PublishingHouseBookDTO
    {
        public int IdPublishingHouse { get; set; }
        public string Name { get; set; }
        public List<BookItemDTO> Books { get; set; }
    }
}
