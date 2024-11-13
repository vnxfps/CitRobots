public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    public bool Deletado { get; set; }
}