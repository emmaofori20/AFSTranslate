using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AFSTranslate.Models
{
    public class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReponseId { get; set; }
        public string? TranslatedText { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public int StatusId { get; set; }  // Foreign key
        public string UserId { get; set; }    // Foreign key
        public int TranslationId { get; set; }  // Foreign key

        public Status Status { get; set; }       // Navigation property
        public ApplicationUser User { get; set; }           // Navigation property
        public TranslationType Translation { get; set; }
    }
}
