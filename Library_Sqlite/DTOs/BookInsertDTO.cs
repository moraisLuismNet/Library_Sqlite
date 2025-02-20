using Library.Validators;

namespace Library.DTOs
{
    public class BookInsertDTO
    {
        public string Title { get; set; }

        [NonNegativePagesValidation]
        public int Pages { get; set; }

        public decimal Price { get; set; }

        [WeightFileValidation(MaximumWeightInMegaBytes: 4)]
        [ValidationFileType(groupFileType: GroupFileType.Image)]
        public IFormFile? Photo { get; set; }

        public bool Discontinued { get; set; }
        public int AuthorId { get; set; }
        public int PublishingHouseId { get; set; }
    }
}
