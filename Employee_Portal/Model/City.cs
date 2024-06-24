using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Portal.Model
{
    public class City
    {
        [Key]
        public int Row_Id { get; set; }
        [ForeignKey("stateId")]
        public int stateId { get; set; }
        public string CityName { get; set; }

    }
}
