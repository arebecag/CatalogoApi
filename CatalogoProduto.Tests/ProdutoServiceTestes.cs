using CatalogoProduto.Domain.Interfaces;
using CatalogoProduto.Domain.Models;
using CatalogoProduto.Domain.Services;
using Moq;

namespace CatalogoProduto.Tests.ProdutoServiceTestes
{
    public class ProdutoServiceTestes
    {
        private Mock<IProdutoRepository> _repositorioMock;
        private ProdutoService _produtoService;

        [SetUp]
        public void Setup()
        {
            _repositorioMock = new Mock<IProdutoRepository>();
            _produtoService = new ProdutoService(_repositorioMock.Object);
        }

        [Test]
        public async Task AdicionarProduto_DeveAdicionarProduto()
        {
            // Preparação
            var produto = new Produto { Nome = "Produto Teste", Valor = 100.00m, DataInclusao = DateTime.Now };
            _repositorioMock.Setup(r => r.AddAsync(produto)).Returns(Task.CompletedTask);

            // Ação
            await _produtoService.AddAsync(produto);

            // Verificação
            _repositorioMock.Verify(r => r.AddAsync(produto), Times.Once);
        }

        [Test]
        public async Task AtualizarProduto_DeveAtualizarProduto()
        {
            var produto = new Produto { IdProduto = 1, Nome = "Produto Atualizado", Valor = 200.00m, DataInclusao = DateTime.Now };
            _repositorioMock.Setup(r => r.GetByIdAsync(produto.IdProduto)).ReturnsAsync(produto);
            _repositorioMock.Setup(r => r.UpdateAsync(produto)).Returns(Task.CompletedTask);

            await _produtoService.UpdateAsync(produto);

            _repositorioMock.Verify(r => r.UpdateAsync(produto), Times.Once);
        }

        [Test]
        public async Task AtualizarProdutoDeveLancarExceptionQuandoNomeJaCadastrado()
        {
            var produtoExistente = new Produto { IdProduto = 1, Nome = "Produto Existente", Valor = 100.00m, DataInclusao = DateTime.Now };
            var produtoAtualizado = new Produto { IdProduto = 1, Nome = "Produto Atualizado", Valor = 200.00m, DataInclusao = DateTime.Now };

            _repositorioMock.Setup(r => r.GetByIdAsync(produtoAtualizado.IdProduto)).ReturnsAsync(produtoExistente);
            _repositorioMock.Setup(r => r.GetByNameAsync(produtoAtualizado.Nome)).ReturnsAsync(produtoExistente);

            var ex = Assert.ThrowsAsync<Exception>(async () => await _produtoService.UpdateAsync(produtoAtualizado));
            Assert.That(ex.Message, Is.EqualTo("Nome já cadastrado, não é possível cadastrar mais de um produto com o nome igual."));
        }

        [Test]
        public async Task DeletarProduto_DeveDeletarProduto()
        {
            var produtoId = 1;
            _repositorioMock.Setup(r => r.GetByIdAsync(produtoId)).ReturnsAsync(new Produto { IdProduto = produtoId });
            _repositorioMock.Setup(r => r.DeleteAsync(produtoId)).Returns(Task.CompletedTask);

            await _produtoService.DeleteAsync(produtoId);

            _repositorioMock.Verify(r => r.DeleteAsync(produtoId), Times.Once);
        }

        [Test]
        public void ExcluirProduto_DeveLancarNullReferenceException_QuandoProdutoNaoForEncontrado()
        {
            var produtoId = 1;
            _repositorioMock.Setup(r => r.GetByIdAsync(produtoId)).ReturnsAsync((Produto)null);

            var ex = Assert.ThrowsAsync<NullReferenceException>(async () => await _produtoService.DeleteAsync(produtoId));
            Assert.That(ex.Message, Is.EqualTo("Produto não encontrado."));
        }

        [Test]
        public async Task ObterProdutoPorId_DeveRetornarProduto()
        {
            var produto = new Produto { IdProduto = 1, Nome = "Produto Teste", Valor = 100.00m, DataInclusao = DateTime.Now };
            _repositorioMock.Setup(r => r.GetByIdAsync(produto.IdProduto)).ReturnsAsync(produto);

            var resultado = await _produtoService.GetByIdAsync(produto.IdProduto);

            Assert.That(resultado, Is.EqualTo(produto));
        }

        [Test]
        public async Task ObterTodosProdutos_DeveRetornarTodosProdutos()
        {
            var produtos = new List<Produto>
            {
                new Produto { IdProduto = 1, Nome = "Produto 1", Valor = 100.00m, DataInclusao = DateTime.Now },
                new Produto { IdProduto = 2, Nome = "Produto 2", Valor = 200.00m, DataInclusao = DateTime.Now }
            };
            _repositorioMock.Setup(r => r.GetAllAsync()).ReturnsAsync(produtos);

            var resultado = await _produtoService.GetAllAsync();

            Assert.That(resultado, Is.EqualTo(produtos));
        }

        [Test]
        public async Task ObterProdutosPaginados_DeveRetornarProdutosPaginados()
        {
            var produtos = new List<Produto>
            {
                new Produto { IdProduto = 1, Nome = "Produto 1", Valor = 100.00m, DataInclusao = DateTime.Now },
                new Produto { IdProduto = 2, Nome = "Produto 2", Valor = 200.00m, DataInclusao = DateTime.Now }
            };
            var totalRegistros = 2;
            _repositorioMock.Setup(r => r.GetPagedAsync(1, 2)).ReturnsAsync((produtos, totalRegistros));

            var (resultado, total) = await _produtoService.GetPagedAsync(1, 2);

            Assert.That(resultado, Is.EqualTo(produtos));
            Assert.That(total, Is.EqualTo(totalRegistros));
        }
    }
}
