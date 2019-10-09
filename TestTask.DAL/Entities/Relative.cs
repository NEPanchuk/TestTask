using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.DAL.Entities
{
    public class Relative
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public User User { get; set; }
    }
}