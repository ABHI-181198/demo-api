using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Portal.Model
{
    public class EmployeeMaster
    {
        [Key]
        public int Row_Id { get; set; }
        [StringLength(8)]
        public string EmployeeCode { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(maximumLength: 50)]
        public string FirstName { get; set; }
        [StringLength(maximumLength: 50)]
        public string LastName { get; set; }
        [ForeignKey("countryId")]
        public int countryId { get; set; }
        [ForeignKey("stateId")]
        public int stateId { get; set; }
        [ForeignKey("cityId")]
        public int cityId { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        //[RegularExpression(@"^[\w\.-]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address format")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid mobile number format")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "PAN number is required")]
        [RegularExpression(@"[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessage = "Invalid PAN number format")]
        public string PanNumber { get; set; }
        [Required(ErrorMessage = "Passport number is required")]
        [RegularExpression(@"^[A-Z]{2}\d{6}$", ErrorMessage = "Invalid passport number format")]
        public string PassportNumber { get; set; }
        public string ProfileImage { get; set; }
        public int Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime DateOfJoinee { get; set; }
        public DateTime UpdatedDate { get; set;}
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime DeletedDate { get; set; }

        [NotMapped]
        public IFormFile ?Imagefile { get; set; }



    }
}
