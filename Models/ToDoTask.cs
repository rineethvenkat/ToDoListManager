using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoListManager.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
    }
}
