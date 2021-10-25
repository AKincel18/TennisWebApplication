using System;
using System.ComponentModel.DataAnnotations;

namespace TennisApplication.Dtos.Tournament
{
    [Serializable]
    public class TournamentReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Place { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Draw size")] public int DrawSize { get; set; }

        public bool Completed { get; set; }

        public bool Started { get; set; }
    }
}