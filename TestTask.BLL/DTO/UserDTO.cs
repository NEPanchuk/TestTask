using System;

namespace TestTask.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }
        public bool Gender { get; set; }
        public int? ParentId { get; set; }
    }
}
