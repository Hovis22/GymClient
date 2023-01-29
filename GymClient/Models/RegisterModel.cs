using System.ComponentModel.DataAnnotations;

namespace GymClient.Models
{
    public class RegisterModel
    {


        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }

        [Required]
        [Phone]
        [StringLength(100)]
     //   [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }


        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [Required]
        [StringLength(50)]
        public string Gender { get; set; }


        [Required]
        [StringLength(100,MinimumLength = 8)]
        public string Password { get; set; }

    }
}
