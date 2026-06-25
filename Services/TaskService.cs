using System;
using System.Collections.Generic;
using System.Text;
using ToDoListManager.Models;
using System.Text.Json;

namespace ToDoListManager.Services
{
    public class TaskService
    {
        private readonly string filePath = "Data/tasks.json";
        private List<ToDoTask> tasks = new();

        /// <summary>
        /// Initializes a new instance of the TaskService class and loads existing tasks from file.
        /// </summary>
        public TaskService()
        {
            LoadTasks();
        }

        /// <summary>
        /// Loads all tasks from the JSON file into memory.
        /// If the file doesn't exist, initializes an empty task list.
        /// </summary>
        private void LoadTasks()
        {
            if(File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                tasks = JsonSerializer.Deserialize<List<ToDoTask>>(json) ?? new List<ToDoTask>();
            }
        }

        /// <summary>
        /// Saves all tasks in memory to the JSON file with formatted indentation.
        /// </summary>
        private void SaveTasks()
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Adds a new task with the specified title and due date.
        /// </summary>
        /// <param name="title">The title of the task.</param>
        /// <param name="dueDate">The due date for the task.</param>
        public void AddTask(string title, DateTime dueDate)
        {
            var task = new ToDoTask
            {
                Id = tasks.Count + 1,
                Title = title,
                DueDate = dueDate,
                IsCompleted = false
            };

            tasks.Add(task);
            SaveTasks();
        }

        /// <summary>
        /// Retrieves all tasks from the current list.
        /// </summary>
        /// <returns>A list of all tasks.</returns>
        public List<ToDoTask> GetAllTasks() => tasks;

        /// <summary>
        /// Marks a task as completed by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to mark as completed.</param>
        public bool CompleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if(task != null)
            {
                task.IsCompleted = true;
                SaveTasks();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes a task by its ID and reindexes remaining tasks to maintain sequential IDs.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        public bool DeleteTask(int id)
        {
            var initialCount = tasks.Count;
            tasks.RemoveAll(t => t.Id == id);
            if(tasks.Count < initialCount)
            {
                //Reassign IDs to maintain order
                for (int i = 0; i < tasks.Count; i++)
                {
                    tasks[i].Id = i + 1;
                }
                SaveTasks();
                return true;
            }
            return false;

        }
    }
}
