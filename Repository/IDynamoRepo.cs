using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataFilters;
using dotnetPoc.Domain.Dto;

namespace dotnetPoc.Repository
{
    public interface IDynamoRepo
    {
        ParametrosDeBusca ObterComFiltros(FiltersParamsInput filtros);
    }
}