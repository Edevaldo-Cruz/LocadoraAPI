using LocadoraAPI.Model;

namespace LocadoraAPI.Model
{
    public class Locacao
    {
        public int Id { get; set; }
        public int Id_Cliente { get; set; }
        public int Id_Filme { get; set; }
        public DateTime DataLocacao { get; set; }
        public DateTime DataDevolucao { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Filme Fillme { get; set; }

    }
}
