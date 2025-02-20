using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class PublishingHouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPublishingHouse { get; set; }

        public string Name { get; set; }

        // Relationship with books: a publisher can have several books
        public virtual ICollection<Book> Books { get; set; }
    }
}