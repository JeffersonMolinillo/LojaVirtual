﻿using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Login;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private IEnderecoEntregaRepository _enderecoEntregaRepository;
        private IClienteRepository _repositoryCliente;
        private LoginCliente _loginCliente;

        public HomeController(IEnderecoEntregaRepository enderecoEntregaRepository, IClienteRepository repositoryCliente, LoginCliente loginCliente)
        {
            _enderecoEntregaRepository = enderecoEntregaRepository;
            _repositoryCliente = repositoryCliente;
            _loginCliente = loginCliente;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Models.Cliente cliente, string returnUrl = null)
        {
            Models.Cliente clienteDB = _repositoryCliente.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null)
            {
                _loginCliente.Login(clienteDB);

                if (returnUrl == null)
                {
                    return new RedirectResult(Url.Action(nameof(Painel)));
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não encontrado, verifique o e-mail e senha digitado!";
                return View();
            }
        }


        [HttpGet]
        [ClienteAutorizacao]
        public IActionResult Painel()
        {
            return new ContentResult() { Content = "Este é o Painel do Cliente!" };
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastroCliente([FromForm] Models.Cliente cliente, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _repositoryCliente.Cadastrar(cliente);
                _loginCliente.Login(cliente);

                TempData["MSG_S"] = "Cadastro realizado com sucesso!";
                if (returnUrl == null)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            return View();
        }


        [HttpGet]
        public IActionResult CadastrarEnderecoEntrega()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarEnderecoEntrega([FromForm] EnderecoEntrega enderecoEntrega, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                enderecoEntrega.ClienteId = _loginCliente.GetCliente().Id;
                _enderecoEntregaRepository.Cadastrar(enderecoEntrega);
                if (returnUrl == null)
                {

                }
                else
                {
                    return LocalRedirectPermanent(returnUrl);
                }
            }
            return View();
        }
    }
}
