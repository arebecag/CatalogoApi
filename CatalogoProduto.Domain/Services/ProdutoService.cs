using CatalogoProduto.Domain.Interfaces;
using CatalogoProduto.Domain.Models;

namespace CatalogoProduto.Domain.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task AddAsync(Produto produto)
        {
            if (await NomeExistente(produto.Nome))
            {
                throw new Exception("Nome já cadastrado, não é possível cadastrar mais de um produto com o nome igual.");
            }

            await _produtoRepository.AddAsync(produto);
        }

        public async Task UpdateAsync(Produto produto)
        {
            var produtoExistente = await _produtoRepository.GetByIdAsync(produto.IdProduto);

            if (produtoExistente == null)
            {
                throw new NullReferenceException("Produto não encontrado.");
            }

            if (await NomeExistente(produto.Nome) && produtoExistente.Nome != produto.Nome)
            {
                throw new Exception("Nome já cadastrado, não é possível cadastrar mais de um produto com o nome igual.");
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Valor = produto.Valor;
            produtoExistente.DataInclusao = produto.DataInclusao;

            await _produtoRepository.UpdateAsync(produtoExistente);
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
            {
                throw new NullReferenceException("Produto não encontrado.");
            }
            await _produtoRepository.DeleteAsync(id);
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
            {
                throw new NullReferenceException("Produto não encontrado.");
            }
            return produto;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _produtoRepository.GetAllAsync();
        }

        public async Task<(IEnumerable<Produto>, int)> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _produtoRepository.GetPagedAsync(pageNumber, pageSize);
        }

        private async Task<bool> NomeExistente(string nome)
        {
            var produto = await _produtoRepository.GetByNameAsync(nome);
            return produto != null;
        }
    }
}