using System.ComponentModel.DataAnnotations;

namespace SignalR_Chat.Dtos
{
    public class UserDto
    {
        [Required]
        [StringLength(12, MinimumLength = 3, ErrorMessage = "Name must be at least {2} and maximum {1} characters")]
        public string Name { get; set; }
    }
}
