using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApplication.Models
{
    [Serializable]
    public enum Winner
    {
        [NotMapped]
        [Display(Name = "1")]
        One,
        [NotMapped]
        [Display(Name = "2")]
        Two
    }
}