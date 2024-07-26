using System.ComponentModel.DataAnnotations;

namespace CatalogoProduto.Api.ViewModels
{
    public class ProdutoViewModel
    {
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do produto deve ter entre 3 e 100 caracteres.")]
        public string Nome { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do produto deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data de inclusão é obrigatória.")]
        public DateTime DataInclusao { get; set; }
    }
}
