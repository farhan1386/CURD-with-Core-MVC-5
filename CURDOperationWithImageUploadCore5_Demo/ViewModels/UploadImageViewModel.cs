using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CURDOperationWithImageUploadCore5_Demo.ViewModels
{
    public class UploadImageViewModel
    {
        [Required]
        [Display(Name = "Image")]
        public IFormFile SpeakerPicture { get; set; }
    }
}
