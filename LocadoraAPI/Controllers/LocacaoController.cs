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
    [Route("api/[controller]")]
    [ApiController]
    public class LocacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocacaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Locacao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetLocacoes()
        {
            return await _context.Locacoes.ToListAsync();
        }

        // GET: api/Locacao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Locacao>> GetLocacao(int id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);

            if (locacao == null)
            {
                return NotFound();
            }

            return locacao;
        }

        // PUT: api/Locacao/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Locacao       
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

            if(filmeEscolhido.Lancamento == 1)
            {
                novaLocacao.DataDevolucao = dataAtual.AddDays(2);
            }
            else
            {
                novaLocacao.DataDevolucao = dataAtual.AddDays(3);
            }

            novaLocacao.Cliente = cliente;
            novaLocacao.Fillme = filmeEscolhido;

            _context.Locacoes.Add(novaLocacao);
            await _context.SaveChangesAsync();

            return Ok(novaLocacao);
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

        private bool LocacaoExists(int id)
        {
            return _context.Locacoes.Any(e => e.LocacaoId == id);
        }
    }
}
