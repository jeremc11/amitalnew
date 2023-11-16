using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskSystem.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime TargetDate { get; set; }
        public int UserId { get; set; }
    }
}
