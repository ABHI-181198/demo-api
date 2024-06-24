using System.ComponentModel.DataAnnotations;

namespace Employee_Portal.Model
{
    public class Country
    {
        [Key]
        public int Row_Id { get; set; } 
        public string CountryName { get; set; }


    }
}
