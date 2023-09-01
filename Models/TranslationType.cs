using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFSTranslate.Models
{
    public class TranslationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TranslationTypeId { get; set; }
        public string ?Name { get; set; }
        public ICollection<Response> Responses { get; set; }

    }
}
