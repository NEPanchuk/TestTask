using System;
using System.Collections.Generic;

namespace TestTask.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }
        public bool Gender { get; set; }
        public int? ParentId { get; set; }

        public ICollection<Relative> Relatives { get; set; }
    }
}