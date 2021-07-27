using AutoMapper;
using LojaVirtual.Controllers.Base;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Libraries.Cookie;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Gerenciador.Frete;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LojaVirtual.Controllers
{
    public class PagamentoController : BaseController
    {
        private Cookie _cookie;
        public PagamentoController(Cookie cookie, CookieCarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wsCorreios, CalcularPacote calcularPacote, CookieFrete cookieValorPrazoFrete) : base(carrinhoCompra, produtoRepository, mapper, wsCorreios, calcularPacote, cookieValorPrazoFrete)
        {
            _cookie = cookie;
        }


        [ClienteAutorizacao]
        public IActionResult Index()
        {
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consultar("Carrinho.TipoFrete", false);
            if (tipoFreteSelecionadoPeloUsuario != null)
            {
                //var frete = _cookieFrete.Consultar().Where(a => a.ListaValores == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();

                //if (frete != null)
                //{
                //    ViewBag.Frete = frete; //passando o frete para a tela através di viewBag
                //    List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

                //    return View(produtoItemCompleto);
                //} 
            }

            TempData["MSG_e"] = Mensagem.MSG_E009;
            return RedirectToAction("Index", "CarrinhoCompra");
        }
    }
}


