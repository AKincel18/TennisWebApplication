﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TennisApplication.Models
{
    [Serializable]
    public class Tournament
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Place { get; set; }
        
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }
        
        public int DrawSize { get; set; }
        
        public bool Completed { get; set; }
    }
}