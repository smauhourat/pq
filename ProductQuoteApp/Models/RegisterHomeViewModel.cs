using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class RegisterHomeViewModel
    {
        [Display(Name = "Razon Social")]
        [Required(ErrorMessage = "El campo es Razon Social Obligatorio")]
        public string Company { get; set; }

        [Display(Name = "Sector")]
        [Required(ErrorMessage = "El campo es Sector Obligatorio")]
        public string Department { get; set; }

        [Display(Name = "Domicilio Legal")]
        [Required(ErrorMessage = "El campo es Domicilio Legal Obligatorio")]
        public string LegalAddress { get; set; }

        [Display(Name = "Nombre del Contacto")]
        [Required(ErrorMessage = "El campo es Nombre del Contacto Obligatorio")]
        public string ContactName { get; set; }

        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "El campo es Cargo Obligatorio")]
        public string JobPosition { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El campo es Telefono Obligatorio")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mail")]
        [Required(ErrorMessage = "El campo es Mail Obligatorio")]
        public string Email { get; set; }
    }
}