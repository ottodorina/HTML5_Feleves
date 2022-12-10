using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HTML5_Feleves.Models
{
    public class Table
    {
        [NotMapped]
        int id = 0;

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Capacity { get; set; }
        public bool Reserved { get; set; }
        public Table()
        {
            Id = id;
            Reserved = false;
            ++id;
        }
    }
}

