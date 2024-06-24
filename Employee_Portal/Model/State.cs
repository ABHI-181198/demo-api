using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Portal.Model
{
    public class State
    {
        [Key]
        public int Row_Id { get; set; }

        [ForeignKey("countryId")]
        public int countryId { get; set; }
        public string StateName { get; set; }
    }
}
