using System.ComponentModel.DataAnnotations;

namespace CRUDApplicationProject.Models
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }

        [Required(ErrorMessage ="Person Name can't be blank ")]
        [StringLength(40,MinimumLength =3,ErrorMessage ="Name must be 2-30 characters")]
        public string? PersonName { get; set; }


        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public string? Email { get; set; }


        [Required(ErrorMessage ="Date Of Birth is Required")]
        public DateTime? DateOfBirth { get; set; }


        [Required(ErrorMessage ="Age is Required")]
        
        public int? Age { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? CountryName { get; set; }
        public string? Address { get; set; }
    }
}
