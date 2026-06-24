using System;
using System.Collections.Generic;
using System.Text;
using ToDoListManager.Models;
using System.Text.Json;
using System.Reflection;
using Microsoft.VisualBasic;

namespace ToDoListManager.Services
{
    public class TaskService
    {
        private readonly string filePath = "Data/tasks.json";
        private List<ToDoTask> tasks = new();

        public TaskService()
        {
            
        }

        private void LoadTasks()
        {
            if(File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                tasks = JsonSerializer.Deserialize<List<ToDoTask>>(json) ?? new List<ToDoTask>();
            }
        }

        private void SaveTasks()
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

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

        public List<ToDoTask> GetAllTasks() => tasks;

        public void CompleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if(task != null)
            {
                task.IsCompleted = true;
                SaveTasks();
            }
        }

        public void DeleteTask(int id)
        {
            tasks.RemoveAll(t => t.Id == id);
            SaveTasks();
        }
    }
}
