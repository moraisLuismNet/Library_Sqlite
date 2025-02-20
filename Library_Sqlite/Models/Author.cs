using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAuthor { get; set; }

        public string Name { get; set; }

        // Relationship with books: an author may have several books
        public virtual ICollection<Book> Books { get; set; }
    }
}
