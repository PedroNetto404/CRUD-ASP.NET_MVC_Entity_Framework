using Microsoft.AspNetCore.Mvc.Razor;
using ProjFinal.Business.Models.Enums;

namespace ProjFinal.WEB.Extensions
{
    public static class RazorExtensions
    {
        public static string FormataDocumento(this RazorPage page, int tipoPessoa, string documento)
        {
            return tipoPessoa == (int)TipoFornecedor.PessoaFisica ?
                Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00") :
                Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00"); 
        }
    }
}
