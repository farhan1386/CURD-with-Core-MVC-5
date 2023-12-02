using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CURDOperationWithImageUploadCore5_Demo.ViewModels
{
    public class FileUploadViewModel
    {
        [Required]
        [Display(Name = "Image")]
        public IFormFile SpeakerPicture { get; set; }
    }
}
