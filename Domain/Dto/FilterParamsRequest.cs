using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetPoc.Domain.Dto
{
    public class FilterParamsRequest
    {
        public string Cnpj { get; set; }

        public string Date { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Produto { get; set; }

        public string Valor { get; set; }
    }
}