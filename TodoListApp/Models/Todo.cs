using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public enum StatusList
    {
        Created,
        InProgress,
        Completed
    }
    public class Todo
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MinLength(4)]
        public string Name { get; set; }

        [Range(1,5)]
        public int Priority { get; set; }

        public string Status { get; set; }    
        
        public Todo() { }

    }
}
