using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O sobrenome é obrigatório")]
    public string Sobrenome { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido")]
    public string CPF { get; set; }

    [Required(ErrorMessage = "A data de nascimento é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório")]
    public string Endereco { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string Senha { get; set; }

    [DataType(DataType.Password)]
    [Compare("Senha", ErrorMessage = "As senhas não conferem")]
    public string ConfirmarSenha { get; set; }
}