using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HTML5_Feleves.Models
{
    public class Reservation
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Person { get; set; }
        public string Contact { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        [NotMapped]
        public virtual Table Table { get; set; }
        public Reservation()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
