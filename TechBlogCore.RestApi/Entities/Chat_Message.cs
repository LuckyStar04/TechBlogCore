using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBlogCore.RestApi.Entities
{
    public class Chat_Message
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(95)]
        [Column(TypeName = "VARCHAR(95)")]
        public string Blog_UserId { get; set; }
        public Blog_User User { get; set; }

        [Required]
        public int Group { get; set; }

        [Required]
        public bool IsMe { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required, StringLength(30)]
        public string Role { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMTEXT")]
        public string Message { get; set; }
    }
}
