using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenFIS.Models
{
    public class Athlete
    {
        [Key]
        public int FisCode { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Year { get; set; }
        [Required]
        public string Nation { get; set; }
    }
}