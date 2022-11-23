using System.ComponentModel.DataAnnotations;

namespace LocadoraAPI.Model
{
    public class Filme
    {
        public int Id { get; set; }
        [StringLength(100, ErrorMessage = "O campo suporta até 100 caracteres.")]
        public string Titulo { get; set; }
        public int ClassificacaoIndicada { get; set; }
        public byte Lancamento { get; set; }
    }
}
