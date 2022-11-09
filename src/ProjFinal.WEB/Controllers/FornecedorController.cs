using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjFinal.Business.Interfaces.Repositories;
using ProjFinal.Business.Models.Entities;
using ProjFinal.WEB.Models;

namespace ProjFinal.WEB.Controllers
{
   
    public class FornecedorController : BaseController
    {
        public FornecedorController(IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
        }
        protected readonly IMapper _mapper;
        protected readonly IFornecedorRepository _fornecedorRepository;

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodosAsync()));
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            return View(fornecedorViewModel); 

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedorRepository.AdicionarAsync(fornecedor); 

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(Guid id)
        {
           
            var fornecedor = _fornecedorRepository.ObterPorIdAsync(id);

            if (fornecedor == null) return NotFound(); 

            return View(_mapper.Map<FornecedorViewModel>(fornecedor)); 
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel); 

            if (id != fornecedorViewModel.Id) return NotFound();

            Fornecedor fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorRepository.AtualizarAsync(fornecedor);

            return RedirectToAction(nameof(Index)); 
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null) return NotFound(); 

            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            return View(fornecedorViewModel); 
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorRepository.RemoverAsync(id);

            return RedirectToAction(nameof(Index)); 

        }
        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEnderecoAsync(id)); 
        }
        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEnderecoAsync(id)); 
        }
    }
}
