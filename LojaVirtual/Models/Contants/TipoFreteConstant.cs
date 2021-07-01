using System;

namespace LojaVirtual.Models.Contants
{
    public class TipoFreteConstant
    {
        public const string SEDEX = "04014";
        public const string SEDEX10 = "04790";
        public const string PAC = "04510";

        internal static string GetNames(string code)
        {
            foreach (var field in typeof(TipoFreteConstant).GetFields())
            {
                if ((string)field.GetValue(null) == code)
                    return field.Name.ToString();
            }
            return "";
          }
    }
}
