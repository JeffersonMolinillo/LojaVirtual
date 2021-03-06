using AutoMapper;
using LojaVirtual.Controllers.Base;
using LojaVirtual.Libraries.CarrinhoCompra;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Gerenciador.Frete;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Libraries.Seguranca;
using LojaVirtual.Models;
using LojaVirtual.Models.Contants;
using LojaVirtual.Models.ProdutoAgregador;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LojaVirtual.Controllers
{
    public class CarrinhoCompraController : BaseController
    {
        private LoginCliente _loginCliente;

        private IEnderecoEntregaRepository _enderecoEntregaRepository;

        public CarrinhoCompraController(LoginCliente loginCliente, IEnderecoEntregaRepository enderecoEntregaRepository, CookieCarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wsCorreios, CalcularPacote calcularPacote, CookieFrete cookieValorPrazoFrete) : base(carrinhoCompra, produtoRepository, mapper, wsCorreios,calcularPacote,cookieValorPrazoFrete)
        {
            _loginCliente = loginCliente;
            _enderecoEntregaRepository = enderecoEntregaRepository;
        }

        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();

            return View(produtoItemCompleto);
        }

        //Item ID = ID Produto
        public IActionResult AdicionarItem(int id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if (produto == null)
            {
                return View("NaoExisteItem");
            }
            else
            {
                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = 1 };
                _cookieCarrinhoCompra.Cadastrar(item);

                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult AlterarQuantidade(int id, int quantidade)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if (quantidade < 1)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E007 });
            }
            else if (quantidade > produto.Quantidade)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E008 });
            }


            var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = quantidade };
            _cookieCarrinhoCompra.Atualizar(item);
            return Ok();
        }
        public IActionResult RemoverItem(int id)
        {
            _cookieCarrinhoCompra.Remover(new ProdutoItem() { Id = id });
            return RedirectToAction(nameof(Index));
        }

        [ClienteAutorizacao]
        public IActionResult EnderecoEntrega()
        {
            Cliente cliente = _loginCliente.GetCliente();
            IList<EnderecoEntrega> enderecos = _enderecoEntregaRepository.ObterTodosEnderecosEntregaCliente(cliente.Id);
            ViewBag.Cliente = cliente;
            ViewBag.Enderecos = enderecos;

            return View();
        }


        public async Task<IActionResult> CalcularFrete(int cepDestino)
        {
            try
            {
                List<ProdutoItem> produtos = CarregarProdutoDB();


                List<Pacote> pacotes = (List<Pacote>)_calcularPacote.CalcularPacotesDeProduto(produtos);


                ValorPrazoFrete valorPAC = await _WsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.PAC, pacotes);
                ValorPrazoFrete valorSEDEX = await _WsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX, pacotes);
                ValorPrazoFrete valorSEDEX10 = await _WsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX10, pacotes);

                List<ValorPrazoFrete> lista = new List<ValorPrazoFrete>();
                if (valorPAC != null) lista.Add(valorPAC);
                if (valorSEDEX != null) lista.Add(valorSEDEX);
                if (valorSEDEX10 != null) lista.Add(valorSEDEX10);


                var frete =  new Frete()
                {
                    CEP = cepDestino,
                    CodCarrinho = GerarHash(_cookieCarrinhoCompra.Consultar()),
                    ListaValores = lista
                };


                _cookieFrete.Cadastrar(frete);

                return Ok(frete);

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }


        }
    }
}