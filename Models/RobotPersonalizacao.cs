namespace CitRobots.Models
{
    public class RobotPersonalizacao
    {
        public int Id { get; set; }
        public int RoboId { get; set; }
        public Robot Robot { get; set; }
        public int PersonalizacaoId { get; set; }
        public Personalizacao Personalizacao { get; set; }
    }
}