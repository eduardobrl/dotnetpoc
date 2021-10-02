using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetPoc.Domain.Attributes;

namespace dotnetPoc.Domain.Dto
{
    public class FiltersParamsInput
    {
        public string Cnpj { get; set; }

        public string Date { get; set; }

        public string Produto { get; set; }

        public string Status { get; set; }

        public string Name { get; set; }

        public double Valor { get; set; }
    }
}