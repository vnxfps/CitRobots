public class Usuario : BaseEntity
{
    public string Login { get; set; }
    public string Senha { get; set; }
    public string Salt { get; set; }
    public Cliente Cliente { get; set; }
}