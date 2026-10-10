using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Task
    {
        public int Id { set; get; }
        [MaxLength(20)]
        [Unicode(false)]
        public string Title { set; get; }
        [MaxLength(100)]
        [Unicode(false)]
        public string Description { set; get; }
        public DateTime DeadLine { set; get; }
        [MaxLength(100)]
        [Unicode(false)]
        public string FileName { set; get; }
    }
}
