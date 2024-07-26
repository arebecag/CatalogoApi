using CatalogoProduto.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProduto.Infra
{
    public class ProdutoContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        public ProdutoContext(DbContextOptions<ProdutoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<Produto>().Property(p => p.Nome).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Produto>().Property(p => p.Valor).IsRequired(); //Adicionar tipo decimal
            modelBuilder.Entity<Produto>().Property(p => p.DataInclusao).IsRequired();
        }
    }
}
