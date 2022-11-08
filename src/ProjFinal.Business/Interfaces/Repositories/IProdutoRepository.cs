using ProjFinal.Business.Models.Entities;

namespace ProjFinal.Business.Interfaces.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedorAsync(Guid fornecedorId);
        Task<IEnumerable<Produto>> ObterProdutosFornecedoresAsync();
        Task<Produto> ObterProdutoFornecedorAsync(Guid id); 
    }
}
