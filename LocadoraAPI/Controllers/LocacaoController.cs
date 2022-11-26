using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocadoraAPI.Context;
using LocadoraAPI.Model;

namespace LocadoraAPI.Controllers
{
    [ApiController]
    public class LocacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocacaoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("RetornaTodasLocacoes")]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetLocacoes()
        {
            return await _context.Locacoes.ToListAsync();
        }

        [HttpGet("RetornaLocacaoPorId/{id}")]
        public async Task<ActionResult<Locacao>> GetLocacao(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);

            if (locacao == null)
            {
                return NotFound();
            }

            return locacao;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocacao(int id, Locacao locacao)
        {
            if (id != locacao.LocacaoId)
            {
                return BadRequest();
            }

            _context.Entry(locacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocacaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("AlugarFilme")]
        public async Task<ActionResult<Locacao>> AlugarFilme(int clienteId, int filmeId)
        {
            var filmeEscolhido = _context.Filmes.Find(filmeId);
            var cliente = _context.Clientes.Find(clienteId);

            DateTime dataAtual = DateTime.Now;


            Locacao novaLocacao = new Locacao();
            novaLocacao.ClienteId = clienteId;
            novaLocacao.FilmeId = filmeId;
            novaLocacao.DataLocacao = dataAtual;

            if (filmeEscolhido.Lancamento == 1)
            {
                novaLocacao.DataDevolucao = dataAtual.AddDays(2);
            }
            else
            {
                novaLocacao.DataDevolucao = dataAtual.AddDays(3);
            }

            novaLocacao.Cliente = cliente;
            novaLocacao.Filme = filmeEscolhido;

            _context.Locacoes.Add(novaLocacao);
            await _context.SaveChangesAsync();

            return Ok(novaLocacao);
        }

        [HttpPut("DevolucaoFilme/{locacaoId}")]
        public async Task<ActionResult<Locacao>> DevolverFilme(int locacaoId)
        {
            var locacao = _context.Locacoes.Find(locacaoId);
            locacao.Devolvido = true;

            _context.Locacoes.Update(locacao);
            await _context.SaveChangesAsync();

            return Ok(locacao);
        }

        // DELETE: api/Locacao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocacao(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
            {
                return NotFound();
            }

            _context.Locacoes.Remove(locacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("RetornaLocacacoesAtrasadas")]
        public IActionResult RetornaLocacacoesAtrasadas()
        {
            var locacoes = _context.Locacoes
                .Where(l => l.Devolvido == false && l.DataDevolucao < DateTime.Now)
                .Include(c => c.Cliente)
                .Include(f => f.Filme)
                .ToList();

            return Ok(locacoes);
        }

        [HttpGet("RetornaFilmeNuncaAlugados")]
        public IActionResult RetornaFilmeNuncaAlugados()
        {
            var filmes = _context.Filmes.Select(x => x.FilmeId).ToList();
            var filmesAlugados = _context.Locacoes.Select(x => x.FilmeId).ToList();

            var filmesNuncaAlugados = filmes.Except(filmesAlugados).ToList();

            List<Filme> ListaFilme = new List<Filme>();
            foreach (var filme in filmesNuncaAlugados)
            {
                var filmeAtual = _context.Filmes.Find(filme);
                ListaFilme.Add(filmeAtual);
            }

            return Ok(ListaFilme);
        }

        [HttpGet("RetornaFilmesMaisAlugados")]
        public IActionResult RetornaFilmesMaisAlugados()
        {
            var dataAtual = DateTime.Now;
            var ultimoAno = dataAtual.AddYears(-1);

            var filmesAlugados = _context.Locacoes
                                    .Where(f => f.DataLocacao < dataAtual
                                        && f.DataLocacao > ultimoAno)
                                    .ToList();

            var lista = filmesAlugados
                        .GroupBy(x => x.FilmeId)
                        .Where(g => g.Count() >= 1).OrderByDescending(g => g.Count())
                        .Select(x => x.Key)
                        .ToList().Take(5);


            List<Filme> ListaFilme = new List<Filme>();
            foreach (var filme in lista)
            {
                var filmeAtual = _context.Filmes.Find(filme);
                ListaFilme.Add(filmeAtual);
            }
            return Ok(ListaFilme);
        }


        [HttpGet("RetornaFilmesMenosAlugadosDaSemana")]
        public IActionResult RetornaFilmesMenosAlugadosDaSemana()
        {
            var dataAtual = DateTime.Now;
            var ultimaSemana = dataAtual.AddDays(-7);

            var filmesAlugados = _context.Locacoes
                                    .Where(f => f.DataLocacao < dataAtual
                                        && f.DataLocacao > ultimaSemana)
                                    .ToList();

            var lista = filmesAlugados
                        .GroupBy(x => x.FilmeId)
                        .Where(g => g.Count() >= 1)
                        .OrderByDescending(g => g.Count())
                        .Select(x => x.Key)
                        .Reverse()
                        .ToList();


            List<Filme> ListaFilme = new List<Filme>();
            int contatador = 3;
            foreach (var filme in lista)
            {
                if(contatador != 0)
                {
                    var filmeAtual = _context.Filmes.Find(filme);
                    ListaFilme.Add(filmeAtual);
                    contatador--;
                }
               
            }
            return Ok(ListaFilme);
        }

        [HttpGet("RetornaSegundoCliente")]
        public IActionResult RetornaSegundoCliente()
        {

            var clientes = _context.Locacoes.ToList();

            var lista = clientes
                        .GroupBy(x => x.ClienteId)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key)
                        .ToList().Take(2);

            List<Cliente> ListaCLiente = new List<Cliente>();

            var segundoCliente = _context.Clientes.Find(lista.Last());
            ListaCLiente.Add(segundoCliente);

            return Ok(ListaCLiente);
        }

        private bool LocacaoExists(int id)
        {
            return _context.Locacoes.Any(e => e.LocacaoId == id);
        }
    }
}
