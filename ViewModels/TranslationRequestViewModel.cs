using AFSTranslate.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AFSTranslate.ViewModels
{
    public class TranslationRequestViewModel
    {
        public string TextToTranslate { get; set; }
        [Required]
        public int TranslationTYpeId { get; set; }
        public SelectList Translations { get; set; }
    }

    public class HomeViewModel
    {
        public TranslationRequestViewModel Translation { get; set; }

        public List<Response> Responses { get; set; }
    }

    public class TranslatedResponse
    {
        public string Translated { get; set; }
        public string Text { get; set; }
        public string Translation { get; set; }
    }

    public class StatusCount
    {
        public int Total { get; set; }
    }
    public class TTResponse
    {
        public StatusCount Success { get; set; }
        public TranslatedResponse Contents { get; set; }
    }

}
