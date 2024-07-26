using CatalogoProduto.Domain.Interfaces;
using CatalogoProduto.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProduto.Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProdutoContext _context;

        public ProdutoRepository(ProdutoContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Produto produto)
        {
            await _context.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await GetByIdAsync(id);
            if (produto != null)
            {
                _context.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task UpdateAsync(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Produto>, int)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Produtos.CountAsync();
            var produtos = await _context.Produtos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (produtos, totalRecords);
        }

        public async Task<Produto> GetByNameAsync(string nome)
        {
            var produto = await _context.Produtos
                .Where(x => x.Nome.ToLower() == nome.ToLower())
                .FirstOrDefaultAsync();

            return produto;
        }
    }
}
