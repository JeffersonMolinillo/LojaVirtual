﻿$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
    MudarImagePrincipalProduto();
    MudarQuantidadeProdutoCarrinho();

    MascaraCep();
    

});

function AJAXCalcularFrete() {
    $(".btn-calcular-frete").click(function () {
        var cep = $(".cep").val().replace(".", "").replace("-", "");

        // Ajax
    });
}


function MascaraCep() {
    $(".cep").mask("00.000-000");
}

function NumberToReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('');
    return numero.join(',');
}

function MudarQuantidadeProdutoCarrinho() {
    $("#order .btn-primary").click(function () {
        if ($(this).hasClass("diminuir")) {
            OrquestradorDeAcoesProduto("diminuir", $(this));
        }
        if ($(this).hasClass("aumentar")) {
            OrquestradorDeAcoesProduto("aumentar", $(this));
        }
    });
}










function OrquestradorDeAcoesProduto(operacao, botao) {
    OcultarMensagemDeErro();
    var pai = botao.parent().parent();

    var produtoId = pai.find(".inputProdutoId").val();
    var quantidadeEstoque = parseInt(pai.find(".inputQuantidadeEstoque").val());
    var valorUnitario = parseFloat(pai.find(".inputValorUnitario").val().replace(",", "."));

    var campoQuantidadeProdutoCarrinho = pai.find(".inputQuantidadeProdutoCarrinho");
    var quantidadeProdutoCarrinhoAntiga = parseInt(campoQuantidadeProdutoCarrinho.val());


    var campoValor = botao.parent().parent().parent().parent().parent().find(".price");

    var produto = new ProdutoQuantidadeEValor(produtoId, quantidadeEstoque, valorUnitario, campoQuantidadeProdutoCarrinho, campoValor, quantidadeProdutoCarrinhoAntiga, 0);

    AlteracoesVisuaisDoProdutoCarrinho(produto, operacao);

}

function AlteracoesVisuaisDoProdutoCarrinho(produto, operacao) {
    if (operacao == "aumentar") {
        //if (produto.quantidadeProdutoCarrinhoAntiga == produto.quantidadeEstoque) {
        //    alert("sem estoque");
        //} else
        {
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga + 1;
            AtualizarQuantidadeEValor(produto);
            AJAXComunicarAlteracaoQuantidadeProduto(produto);
        }

    }
    else if (operacao == "diminuir") {
        //if (produto.quantidadeProdutoCarrinhoAntiga == 1) {
        //    alert("quantidade miníma");
        //}
        //else
        {
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga - 1;
            AtualizarQuantidadeEValor(produto);
            AJAXComunicarAlteracaoQuantidadeProduto(produto);

        }
    }
}

function AJAXComunicarAlteracaoQuantidadeProduto(produto) {
    $.ajax({
        type: "GET",
        url: "/CarrinhoCompra/AlterarQuantidade?id=" + produto.produtoId + "&quantidade=" + produto.quantidadeProdutoCarrinhoNova,
        error: function (data) {
            MostrarMensagemDeErro(data.responseJSON.mensagem);
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga;  /*Rolback da quantidade se der erro*/
            AtualizarQuantidadeEValor(produto);
        },
        success: function () {
        }
    });
}

function MostrarMensagemDeErro(mensagem) {
    $(".alert-danger").css("display", "block");
    $(".alert-danger").text(mensagem);
}

function OcultarMensagemDeErro() {
    $(".alert-danger").css("display", "none");
}



function AtualizarQuantidadeEValor(produto) {
    produto.campoQuantidadeProdutoCarrinho.val(produto.quantidadeProdutoCarrinhoNova);

    var resultado = produto.valorUnitario * produto.quantidadeProdutoCarrinhoNova;
    produto.campoValor.text(NumberToReal(resultado));
    AtualizarSubtotal();

}

function AtualizarSubtotal() {
    var Subtotal = 0;
    var TagsComPrice = $(".price");
    TagsComPrice.each(function () {
        var ValorReais = parseFloat($(this).text().replace("R$", "").replace(".", "").replace(" ", "").replace(",", "."));

        Subtotal += ValorReais;

    })
    $(".subtotal").text(NumberToReal(Subtotal));
}


function MudarImagePrincipalProduto() {
            $(".img-small-wrap img").click(function () {
                var Caminho = $(this).attr("src");
                $(".img-big-wrap img").attr("src", Caminho);
                $(".img-big-wrap a").attr("href", Caminho);
            });
        }
function MoverScrollOrdenacao() {
            if (window.location.hash.length > 0) {
                var hash = window.location.hash;
                if (hash == "#posicao-produto") {
                    window.scrollBy(0, 473);
                }
            }
        }


function MudarOrdenacao() {
            $("#ordenacao").change(function () {
                //TODO - Redirecionar para a tela Home/Index passando as QueryString de Ordenação e mantendo a Pagina e a pesquisa.
                var Pagina = 1;
                var Pesquisa = "";
                var Ordenacao = $(this).val();
                var Fragmento = "#posicao-produto";

                var QueryString = new URLSearchParams(window.location.search);
                if (QueryString.has("pagina")) {
                    Pagina = QueryString.get("pagina");
                }
                if (QueryString.has("pesquisa")) {
                    Pesquisa = QueryString.get("pesquisa");
                }
                if ($("#breadcrumb").length > 0) {
                    Fragmento = "";
                }

                var URL = window.location.protocol + "//" + window.location.host + window.location.pathname;

                var URLComParametros = URL + "?pagina=" + Pagina + "&pesquisa=" + Pesquisa + "&ordenacao=" + Ordenacao + Fragmento;
                window.location.href = URLComParametros;

            }


            )
        };





    /*--------------------------------------------------*/


    class ProdutoQuantidadeEValor {
        constructor(produtoId, quantidadeEstoque, valorUnitario, campoQuantidadeProdutoCarrinho, campoValor, quantidadeProdutoCarrinhoAntiga, quantidadeProdutoCarrinhoNova) {
            this.produtoId = produtoId;
            this.quantidadeEstoque = quantidadeEstoque;
            this.valorUnitario = valorUnitario;

            this.quantidadeProdutoCarrinhoAntiga = quantidadeProdutoCarrinhoAntiga;
            this.quantidadeProdutoCarrinhoNova = quantidadeProdutoCarrinhoNova;

            this.campoQuantidadeProdutoCarrinho = campoQuantidadeProdutoCarrinho;
            this.campoValor = campoValor;
        }
    }