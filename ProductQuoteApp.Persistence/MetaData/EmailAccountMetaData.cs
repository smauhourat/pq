using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class EmailAccountMetaData
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email es obligatorio")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Nombre es obligatorio")]
        public string DisplayName { get; set; }

        [Display(Name = "Servidor")]
        [Required(ErrorMessage = "Servidor es obligatorio")]
        public string Host { get; set; }

        [Display(Name = "Puerto")]
        [Required(ErrorMessage = "Puerto es obligatorio")]
        public int Port { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Usuario es obligatorio")]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Contraseña es obligatorio")]
        public string Password { get; set; }

        [Display(Name = "SSL")]
        [Required(ErrorMessage = "SSL es obligatorio")]
        public bool EnableSsl { get; set; }

        [Display(Name = "Credenciales")]
        [Required(ErrorMessage = "Credenciales es obligatorio")]
        public bool UseDefaultCredentials { get; set; }

        [Display(Name = "Principal")]
        public bool IsDefault { get; set; }

    }
}
