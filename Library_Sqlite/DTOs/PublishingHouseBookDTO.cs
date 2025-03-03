namespace Library.DTOs
{
    public class PublishingHouseBookDTO
    {
        public int IdPublishingHouse { get; set; }
        public string NamePublishingHouse { get; set; }

        public int TotalBooks { get; set; }
        public List<BookItemDTO> Books { get; set; }

        public decimal AveragePrices { get; set; }
    }
}
