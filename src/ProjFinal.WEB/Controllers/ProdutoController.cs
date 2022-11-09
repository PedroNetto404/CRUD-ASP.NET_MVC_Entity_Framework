using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjFinal.Business.Interfaces.Repositories;
using ProjFinal.Business.Models.Entities;
using ProjFinal.WEB.Models;

namespace ProjFinal.WEB.Controllers
{
    public class ProdutoController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutoController(IFornecedorRepository fornecedorRepository, IProdutoRepository produtoRepository, IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository; 
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedoresAsync()));
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto == null) return NotFound();

            return View(produto);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            var produtoAtualizacao = await ObterProduto(id);

            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor; 
            produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if (!ModelState.IsValid) return View(produtoViewModel);

            if(produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if(! await UploadFile(produtoViewModel.ImagemUpload, imgPrefixo)) return View(produtoViewModel);

                produtoAtualizacao.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }

            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoRepository.AtualizarAsync(_mapper.Map<Produto>(produtoAtualizacao));

            return RedirectToAction(nameof(Index)); 
        }


        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await ObterProduto(id);
            if (produto == null) return NotFound(); 

            await _produtoRepository.RemoverAsync(id);

            return RedirectToAction(nameof(Index)); 
        }
        
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return View(produtoViewModel);
            
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";

            if (! await UploadFile(produtoViewModel.ImagemUpload, imgPrefixo)) return View(produtoViewModel);

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName; 

            await _produtoRepository.AdicionarAsync(_mapper.Map<Produto>(produtoViewModel));

            return RedirectToAction(nameof(Index));  
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorIdAsync(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodosAsync()); 
            return produto; 
        }
        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodosAsync());

            return produto; 
        }
        private async Task<bool> UploadFile(IFormFile imagemUpload, string imgPrefixo)
        {
            if (imagemUpload.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + imagemUpload.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imagemUpload.CopyToAsync(stream); 
            }
            return true;
        }
    }
}
