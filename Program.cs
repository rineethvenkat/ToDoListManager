// See https://aka.ms/new-console-template for more information


using ToDoListManager.Services;

var service = new TaskService();

while(true)
{
    Console.WriteLine("\n===== To-Do List Manager ======");
    Console.WriteLine("1. Add Task");
    Console.WriteLine("2. View Tasks");
    Console.WriteLine("3. Complete Task");
    Console.WriteLine("4. Delete Task");
    Console.WriteLine("5. Exit");

    Console.Write("Choose an option: ");
    var choice = Console.ReadLine();
    
    switch(choice)
    {
        case "1":
            Console.Write("Enter task title: ");
            var title = Console.ReadLine();

            Console.Write("Enter due date (yyyy-MM-dd): ");
            var dueDateInput = Console.ReadLine();
            if(DateTime.TryParse(dueDateInput, out DateTime dueDate))
            {
                service.AddTask(title, dueDate);
                Console.WriteLine("Task added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
            break;

        case "2":
            var tasks = service.GetAllTasks();
            foreach(var task in tasks)
            {
                var originalColor = Console.ForegroundColor;

                // choose a highlight color only for this task line
                ConsoleColor? highlight = null;
                if (!task.IsCompleted && task.DueDate < DateTime.Now)
                    highlight = ConsoleColor.Red;
                else if (task.IsCompleted)
                    highlight = ConsoleColor.Green;

                // print id in the default color, then the rest in the highlight (if any)
                Console.Write($"{task.Id}. ");
                if (highlight.HasValue)
                    Console.ForegroundColor = highlight.Value;

                Console.WriteLine($"{task.Title} - Due: {task.DueDate.ToShortDateString()} - Completed: {task.IsCompleted}");

                // restore immediately so only this task line was colored
                Console.ForegroundColor = originalColor;
            }
            break;

        case "3":
            Console.Write("Enter task ID to complete: ");
            var idInput = Console.ReadLine();
            if(int.TryParse(idInput, out int id))
            {
                service.CompleteTask(id);
                Console.WriteLine("Task marked as completed!");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
            break;

        case "4":
            Console.Write("Enter task ID to delete: ");
            service.DeleteTask(int.Parse(Console.ReadLine()));
            break;

         case "5":
            return;

        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}
