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
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext dbContext) : base(dbContext) {}

        public async Task<Produto> ObterProdutoFornecedorAsync(Guid id)
        {
            return await Db.Produtos.AsNoTracking()
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id); 

        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedoresAsync()
        {
            return await Db.Produtos.AsNoTracking()
                .Include(p => p.Fornecedor)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedorAsync(Guid fornecedorId)
        {
            return await BuscarAsync(p => p.FornecedorId == fornecedorId); 
        }
    }
}
