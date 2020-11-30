using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppShared.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Le prénom doit être saisi")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Le nom doit être saisi")]
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        [Required(ErrorMessage = "La rue doit être saisie")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = "La rue doit être comprise en 3 et 128 caractères")]
        public string Street { get; set; }
        public string Zipcode { get; set; }
        [Required(ErrorMessage = "La séléction d'un pays est obligatoire")]
        public Country Country { get; set; }
        [Required(ErrorMessage = "La sélection d'un poste est obligatoire")]
        public Job Job { get; set; }
    }
}
