using System;

namespace TaskFlowApp
{
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public string Tag { get; set; }

        public Task()
        {
            Name = "";
            Description = "";
            Status = "До";
            DueDate = DateTime.Now;
            Tag = "";
        }

        public Task(string name, string description, string status, DateTime dueDate, string tag)
        {
            Name = name;
            Description = description;
            Status = status;
            DueDate = dueDate;
            Tag = tag;
        }
    }
}