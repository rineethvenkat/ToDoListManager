// See https://aka.ms/new-console-template for more information


using ToDoListManager.Services;
// Initialize the task service to manage all task operations
var service = new TaskService();
// Main application loop - runs until user chooses to exit
while (true)
{
    // Display the main menu with available options
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
        // Case 1: Add a new task
        case "1":
            Console.Write("Enter task title: ");
            var title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Task title cannot be empty.");
                break;
            }

            Console.Write("Enter due date (yyyy-MM-dd): ");
            var dueDateInput = Console.ReadLine();
            // Validate date format and add task if valid
            if (DateTime.TryParse(dueDateInput, out DateTime dueDate))
            {
                service.AddTask(title, dueDate);
                Console.WriteLine("Task added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid date format.");
            }
            break;

        // Case 2: Display all tasks with color coding
        case "2":
            var tasks = service.GetAllTasks();
            foreach(var task in tasks)
            {
                var originalColor = Console.ForegroundColor;

                // Determine highlight color based on task status
                // Red: overdue tasks (not completed and past due date)
                // Green: completed tasks
                // None: upcoming tasks
                ConsoleColor? highlight = null;
                if (!task.IsCompleted && task.DueDate < DateTime.Now)
                    highlight = ConsoleColor.Red;
                else if (task.IsCompleted)
                    highlight = ConsoleColor.Green;

                // Print task ID in default color
                Console.Write($"{task.Id}. ");
                // Apply highlight color if applicable, then print task details
                if (highlight.HasValue)
                    Console.ForegroundColor = highlight.Value;

                Console.WriteLine($"{task.Title} - Due: {task.DueDate.ToShortDateString()} - Completed: {task.IsCompleted}");

                // restore immediately so only this task line was colored
                Console.ForegroundColor = originalColor;
            }
            break;

        // Case 3: Mark a task as completed
        case "3":
            Console.Write("Enter task ID to complete: ");
            var idInput = Console.ReadLine();
            if(int.TryParse(idInput, out int id))
            {
                // Attempt to complete the task; display appropriate message based on result
                if (service.CompleteTask(id))
                    Console.WriteLine("Task marked as completed!");
                else
                    Console.WriteLine("Task not found.");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
            break;

        // Case 4: Delete a task by ID
        case "4":
            Console.Write("Enter task ID to delete: ");
            var deleteInput = Console.ReadLine();
            if (int.TryParse(deleteInput, out int deleteId))
            {
                // Attempt to delete the task; display appropriate message based on result
                if (service.DeleteTask(deleteId))
                    Console.WriteLine("Task deleted successfully!");
                else
                    Console.WriteLine("Task not found.");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
            break;

        // Case 5: Exit the application
        case "5":
            return;

        // Default case: Invalid menu selection
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}
