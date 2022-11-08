using ProjFinal.Business.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjFinal.Business.Interfaces.Repositories
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        Task<Fornecedor> ObterFornecedorEnderecoAsync(Guid id);
        Task<Fornecedor> ObterFornecedorProdutosEnderecoAsync(Guid id); 
    }
}
