using System.Collections.Generic;

namespace LojaVirtual.Models
{
    public class Frete
    {
        public int CEP { get; set; }
        public string CodCarrinho { get; set; }
        public List<ValorPrazoFrete> ListaValores { get; set; }
    }
}
