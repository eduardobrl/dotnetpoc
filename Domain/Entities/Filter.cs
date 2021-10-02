using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetPoc.Domain.Enums;

namespace dotnetPoc.Domain.Entities
{
    public class Filter
    {
        public object Valor { get; set; }
        public string Chave { get; set; }

        public TiposFiltro Op { get; set; }


        public Filter(string chave, string valor, TiposFiltro op = TiposFiltro.Igual)
        {
            Chave = chave;
            Valor = valor;
            Op = op;
        }
    }
}