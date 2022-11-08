using AutoMapper;
using ProjFinal.Business.Models.Entities;
using ProjFinal.WEB.Models;

namespace ProjFinal.WEB.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap(); 
        }
    }
}
