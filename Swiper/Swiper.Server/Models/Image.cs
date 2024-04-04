using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swiper.Server.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public byte[] Data { get; set; }

        public Image(byte[] data)
        {
            Data = data;
        }
    }
}
