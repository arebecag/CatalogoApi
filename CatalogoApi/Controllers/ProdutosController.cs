using CatalogoProduto.Api.ViewModels;
using CatalogoProduto.Domain.Interfaces;
using CatalogoProduto.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> GetProdutos()
        {
            var produtos = await _produtoService.GetAllAsync();
            var produtosViewModel = produtos.Select(x => new ProdutoViewModel { IdProduto = x.IdProduto, Nome = x.Nome, Valor = x.Valor });
            return Ok(produtosViewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoViewModel>> GetProduto(int id)
        {
            try
            {
                var produto = await _produtoService.GetByIdAsync(id);
                var produtoViewModel = new ProdutoViewModel { IdProduto = produto.IdProduto, Valor = produto.Valor, Nome = produto.Nome };
                return Ok(produtoViewModel);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostProduto([FromBody] ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var produto = new Produto
                {
                    Nome = produtoViewModel.Nome.ToLower(),
                    Valor = produtoViewModel.Valor,
                    DataInclusao = produtoViewModel.DataInclusao
                };

                await _produtoService.AddAsync(produto);
                return Ok("Produto cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, [FromBody] ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.IdProduto)
            {
                return BadRequest("ID do produto não corresponde.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var produtoExistente = await _produtoService.GetByIdAsync(id);

                if (produtoExistente.Nome == produtoViewModel.Nome && produtoExistente.Valor == produtoViewModel.Valor)
                {
                    return Ok("Nenhuma alteração detectada.");
                }

                var produto = new Produto
                {
                    IdProduto = produtoViewModel.IdProduto,
                    Nome = produtoViewModel.Nome,
                    Valor = produtoViewModel.Valor,
                    DataInclusao = produtoViewModel.DataInclusao
                };

                await _produtoService.UpdateAsync(produto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            try
            {
                await _produtoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPagedProdutos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (produtos, totalRecords) = await _produtoService.GetPagedAsync(pageNumber, pageSize);

            var response = new
            {
                Data = produtos,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
            };

            return Ok(response);
        }
    }
}
