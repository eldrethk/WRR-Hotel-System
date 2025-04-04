using System.ComponentModel.DataAnnotations;
using WRR8_0.Extension;

namespace WRR8_0.Models
{
    public class UploadImageViewModel
    {
        [Required]
        [AllowedExtensions(new string[] {".jpg", ".jpeg", ".png"})]
        public IFormFile Image { get; set; }

        public int Id { get; set; }
    }
}
