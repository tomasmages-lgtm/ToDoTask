using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace ToDoList.ViewModels
{
    public class CreatedTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public IFormFile ActualFile { get; set; }
    }
}
