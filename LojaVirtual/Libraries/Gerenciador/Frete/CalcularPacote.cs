﻿using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;
using System.Collections.Generic;

namespace LojaVirtual.Libraries.Gerenciador.Frete
{
    public class CalcularPacote
    {
        public IList<Pacote> CalcularPacotesDeProduto(List<ProdutoItem> produtos)
        {
            List<Pacote> pacotes = new List<Pacote>();

            Pacote pacote = new Pacote();
            foreach (var item in produtos)
            {
                for (int i = 0; i < item.QuantidadeProdutoCarrinho; i++)
                {
                    var peso = pacote.Peso + item.Peso;
                    var comprimento = (pacote.Comprimento > item.Comprimento ? pacote.Comprimento : item.Comprimento);
                    var largura = (pacote.Largura > item.Largura ? pacote.Largura : item.Largura);
                    var altura = pacote.Altura + item.Altura;

                    var dimensao = comprimento + largura + altura;

                    if (peso > 30 || dimensao > 200 || altura > 105 || largura > 105)
                    {
                        pacotes.Add(pacote);
                        pacote = new Pacote();
                    }

                    pacote.Peso = pacote.Peso + item.Peso;
                    pacote.Comprimento = (pacote.Comprimento > item.Comprimento ? pacote.Comprimento : item.Comprimento);
                    pacote.Largura = (pacote.Largura > item.Largura ? pacote.Largura : item.Largura);
                    pacote.Altura = pacote.Altura + item.Altura;
                }
            }
            pacotes.Add(pacote);

            return pacotes;
        }
    }
}
