using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CURDOperationWithImageUploadCore5_Demo.ViewModels
{
    public class UploadImageViewModel
    {
        [Display(Name = "Picture")]
        public IFormFile SpeakerPicture { get; set; }
    }
}
