using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tp2tuller.Data; // Namespace onde está o ApplicationDbContext
using tp2tuller.Models;

namespace tp2tuller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Construtor com injeção de dependência para acessar o DbContext
        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            // Retorna todos os produtos do banco de dados
            return await _context.Produtos.ToListAsync();
        }

        // GET: api/produtos/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            // Busca o produto pelo ID no banco de dados
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return produto;
        }

        // POST: api/produtos
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Adiciona o produto ao banco de dados
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // PUT: api/produtos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, [FromBody] Produto produtoAtualizado)
        {
            if (id != produtoAtualizado.Id)
                return BadRequest("ID do produto não corresponde.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Marca o produto como modificado no DbContext
            _context.Entry(produtoAtualizado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica se o produto existe antes de tentar salvar
                if (!ProdutoExists(id))
                    return NotFound("Produto não encontrado.");
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/produtos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            // Remove o produto do banco de dados
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar se o produto existe
        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(p => p.Id == id);
        }
    }
}
