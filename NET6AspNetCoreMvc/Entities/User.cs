using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET6AspNetCoreMvc.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string? NameSurname { get; set; }

        [Required]
        [StringLength(50)] // Bu değerleri girmediğimizde nvarchar max olmakta.
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool Locked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
