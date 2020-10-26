using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApplication.Models
{
    [Serializable]
    public enum Winner
    {
        [NotMapped]
        One,
        [NotMapped]
        Two
    }
}