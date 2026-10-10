using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using ToDoList.DataAccess;
using ToDoList.ViewModels;


namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        ApplicationDbContext _MyContext = new ApplicationDbContext();
        public IActionResult Index(DateTime? DeadLine, int page = 1) {
            var Tasks = _MyContext.Tasks.ToList();
            if (DeadLine != null) {
                Tasks = Tasks.Where(t => t.DeadLine == DeadLine).ToList();
            }
            Tasks = Tasks.OrderBy(t => t.DeadLine).ToList();
            int SizePage = 3;
            ViewBag.TotalPages = (int)Math.Ceiling(Tasks.Count() / (decimal)SizePage);
            Tasks = Tasks.Skip((page - 1) * SizePage).Take(SizePage).ToList();
            return View(Tasks);
        }

        [HttpGet]
        public IActionResult CreateTask()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateTask(CreatedTask NewTask)
        {

            if (NewTask.ActualFile != null && NewTask.ActualFile.Length > 0) {
                var Task = new Models.Task();
                Task.Title = NewTask.Title;
                Task.Description = NewTask.Description;
                Task.DeadLine = NewTask.DeadLine;
                // i stored the data in the database but i didnt store the actual file on my server yet
                // we must create a file name for my file to avoid the redudncy
                var FileName = Guid.NewGuid().ToString() + "-" + NewTask.ActualFile.FileName;
                // the we must create the path which we store that file in
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", FileName);
                Task.FileName = FileName;
                using (var stream = System.IO.File.Create(FilePath)) {
                    NewTask.ActualFile.CopyTo(stream);
                }
                _MyContext.Tasks.Add(Task);
                _MyContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditTask(int Id) {
            var Task = _MyContext.Tasks.FirstOrDefault(t => t.Id == Id);
            if (Task == null)
            {
                return NotFound();
            }
                return View(Task);
        }
        [HttpPost]
        public IActionResult EditTask(int Id,CreatedTask NewTask)
        {
            var OldTask = _MyContext.Tasks.FirstOrDefault(t => t.Id == Id);
            // replace old data with my task
            OldTask.Title = NewTask.Title;
            OldTask.Description = NewTask.Description;
            OldTask.DeadLine = NewTask.DeadLine;
             
             
            //create file name to prevent the redundcy and then upload this file on my server for (Filepath)
            if (NewTask.ActualFile != null && NewTask.ActualFile.Length > 0) {
                //delete the old path from my server
                if (!string.IsNullOrEmpty(OldTask.FileName)) {
                    var OldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", OldTask.FileName);
                    if (System.IO.File.Exists(OldFilePath))
                    {
                        System.IO.File.Delete(OldFilePath);
                    }
                }

                var FileName = Guid.NewGuid().ToString() + "-" + NewTask.ActualFile.FileName;
                var FilePatrh = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", FileName);
                using (var stream = System.IO.File.Create(FilePatrh)) {
                    NewTask.ActualFile.CopyTo(stream);
                }
                OldTask.FileName = FileName;
            }
            _MyContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteTask(int Id) {
            var DeletedTask = _MyContext.Tasks.FirstOrDefault(t => t.Id == Id);
            if (DeletedTask != null) {
                if (!string.IsNullOrEmpty(DeletedTask.FileName)) {
                    var DeletedTaskPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//uploads", DeletedTask.FileName);
                    if (System.IO.File.Exists(DeletedTaskPath)) System.IO.File.Delete(DeletedTaskPath);
                }
            _MyContext.Tasks.Remove(DeletedTask);
            }
            _MyContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
