using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBook { get; set; }

        public string Title { get; set; }

        public int Pages { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Price { get; set; }

        public string? PhotoCover { get; set; }

        public bool Discontinued { get; set; }

        // Foreign keys for the relationship with Author and Publisher

        [ForeignKey("AuthorId")]
        // Foreign key for the relationship with Author
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        [ForeignKey("PublishingHouseId")]
        // Foreign key for the relationship with the publishing house
        public int PublishingHouseId { get; set; }
        public virtual PublishingHouse PublishingHouse { get; set; }
    }
}

