using AutoMapper;
using LojaVirtual.Controllers.Base;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Libraries.Gerenciador.Frete;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LojaVirtual.Controllers
{
    public class PagamentoController : BaseController
    {

        public PagamentoController(CookieCarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wsCorreios, CalcularPacote calcularPacote, CookieValorPrazoFrete cookieValorPrazoFrete) : base(carrinhoCompra, produtoRepository, mapper, wsCorreios, calcularPacote, cookieValorPrazoFrete)
        {

        }
        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

            return View(produtoItemCompleto);
        }
    }
}

