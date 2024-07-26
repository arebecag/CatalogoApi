using CatalogoProduto.Domain.Models;

namespace CatalogoProduto.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(int id);
        Task<Produto> GetByIdAsync(int id);
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<(IEnumerable<Produto>, int)> GetPagedAsync(int pageNumber, int pageSize);
        Task<Produto> GetByNameAsync(string nome);
    }
}
