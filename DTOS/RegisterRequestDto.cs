using System.ComponentModel.DataAnnotations;

namespace ControleDeAcesso.DTOS
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(20, ErrorMessage = "Nome não pode exceder 20 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [MaxLength(50, ErrorMessage = "Email não pode exceder 50 caracteres.")]
        [MinLength(5, ErrorMessage = "Email deve ter pelo menos 5 caracteres.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Senha deve ter pelo menos 8 caracteres.")]
        [MaxLength(100, ErrorMessage = "Senha não pode exceder 100 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}
