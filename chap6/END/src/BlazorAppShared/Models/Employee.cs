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
        [Required(ErrorMessage = "La saisie de l'adresse est nécessaire")]
        public AddressModel Address { get; set; }
        [Required(ErrorMessage = "La sélection d'un poste est obligatoire")]
        public Job Job { get; set; }
    }
}
