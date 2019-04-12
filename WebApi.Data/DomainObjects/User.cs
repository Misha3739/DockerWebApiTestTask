using System.ComponentModel.DataAnnotations;

namespace WebApi.Data.DomainObjects
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Email { get; set; }
    }
}