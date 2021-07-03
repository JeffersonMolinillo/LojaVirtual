using AutoMapper;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Libraries.Gerenciador.Frete;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LojaVirtual.Controllers.Base
{
    public abstract class BaseController : Controller
    {

        protected CookieCarrinhoCompra _cookieCarrinhoCompra;
        protected CookieValorPrazoFrete _cookieValorPrazoFrete;
        protected IProdutoRepository _produtoRepository;
        protected IMapper _mapper;
        protected WSCorreiosCalcularFrete _WsCorreios;
        protected CalcularPacote _calcularPacote;

        public BaseController(CookieCarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wsCorreios, CalcularPacote calcularPacote, CookieValorPrazoFrete cookieValorPrazoFrete)
        {
            _cookieCarrinhoCompra = carrinhoCompra;
            _cookieValorPrazoFrete = cookieValorPrazoFrete;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _WsCorreios = wsCorreios;
            _calcularPacote = calcularPacote;
        }
      
        protected List<ProdutoItem> CarregarProdutoDB()
        {
            List<ProdutoItem> produtoItemNoCarrinho = _cookieCarrinhoCompra.Consultar();

            List<ProdutoItem> produtoItemCompleto = new List<ProdutoItem>();

            foreach (var item in produtoItemNoCarrinho)
            {
                //TODO - Implementar AutoMapper...
                Produto produto = _produtoRepository.ObterProduto(item.Id);

                ProdutoItem produtoItem = _mapper.Map<ProdutoItem>(produto);

                produtoItem.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;

                produtoItemCompleto.Add(produtoItem);
            }

            return produtoItemCompleto;
        }


    }
}
