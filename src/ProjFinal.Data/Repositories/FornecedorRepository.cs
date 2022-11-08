using Microsoft.EntityFrameworkCore;
using ProjFinal.Business.Interfaces.Repositories;
using ProjFinal.Business.Models.Entities;
using ProjFinal.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjFinal.Data.Repositories
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Fornecedor> ObterFornecedorEnderecoAsync(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id); 
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEnderecoAsync(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Produtos)
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
