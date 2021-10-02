using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DataFilters;
using dotnetPoc.Domain.Attributes;
using dotnetPoc.Domain.Dto;

namespace dotnetPoc.Repository
{
    public class DynamoDbRepository : IDynamoRepo
    {
        public DynamoDbRepository()
        {
        }

        public ParametrosDeBusca ObterComFiltros(List<Filter> filterlist)
        {
            var parametrosDeBusca = ObterParametrosDeBusca(filterlist);

            return parametrosDeBusca;

        }


        private ParametrosDeBusca ObterParametrosDeBusca(List<Filter> ListaDeFitros)
        {
            List<Index> indices = new List<Index>();
            List<Filtro> filtros = new List<Filtro>();

            foreach (var filtro in ListaDeFitros)
            {
                var nomeindice = typeof(OperacaoDb).GetProperty(filtro.Field).GetCustomAttribute<DynamoDbIndexAttribute>()?.Value;
                var prop = typeof(OperacaoDb).GetProperty(filtro.Field).GetCustomAttribute<DynamoDbPropertyAttribute>()?.Value;

                var novoFiltro = new Filtro(prop, filtro.Value, filtro.Operator);

                if(nomeindice != null && filtro.Operator == FilterOperator.EqualTo)
                {
                    var filtroSk = ObterRangeKeyParaIndice(ListaDeFitros, nomeindice);

                    indices.Add(new Index {
                        IndexName = nomeindice,
                        Pk = novoFiltro,
                        Sk = filtroSk
                    });
                }

                filtros.Add(novoFiltro);
              
            }

            var indice = ObterIndicePreferencial(indices);


            filtros.Remove(indice.Pk);
            filtros.Remove(indice.Sk);

            return new ParametrosDeBusca {
                Filtros = filtros,
                Indice = indice
            };

        }

        private Index ObterIndicePreferencial(List<Index> indices)
        {
            if(indices.Count == 0)
            {
                return null;
            }

            if(indices.Count == 1)
            {
                return indices.FirstOrDefault<Index>();
            }

            var indicesComSk = (from indice in indices
                where indice.Sk != null
                select indice).ToList();

            if(indicesComSk.Count == 0)
            {
                return ObterPrioritário(indices);
            }

            if(indicesComSk.Count == 1)
            {
                return indicesComSk.FirstOrDefault<Index>();
            }

            return ObterPrioritário(indicesComSk);

        }

        private Index ObterPrioritário(List<Index> indices)
        {
            return (from indice in indices 
                orderby OperacaoDb.ListaDePrioridades.GetValueOrDefault(indice.IndexName)
                select indice).ToList().FirstOrDefault<Index>();
        }

        private Filtro ObterRangeKeyParaIndice(List<Filter> ListaDeFitros, string indice)
        {
            foreach (var filtro in ListaDeFitros)
            {
                var range = typeof(OperacaoDb).GetProperty(filtro.Field).GetCustomAttribute<DynamoDbRangeAttribute>()?.Value;
                var prop = typeof(OperacaoDb).GetProperty(filtro.Field).GetCustomAttribute<DynamoDbPropertyAttribute>()?.Value;

                if(indice == range)
                {
                    return new Filtro(prop, filtro.Value, filtro.Operator);
                }
                
            }

            return null;
        }

       
    }

    public class Index
    {
        public string IndexName { get; set; }

        public Filtro Pk { get; set; }

        public Filtro Sk { get; set; }

    }


    public class Filtro
    {
        public string Propriedade { get; set; }

        public FilterOperator Operacao { get; set; }

        public object Valor { get; set; }

        public Filtro(string prop, object valor, FilterOperator op = FilterOperator.EqualTo)
        {
            Propriedade=prop;
            Valor=valor;
            Operacao=op;

        }
    }

    public class ParametrosDeBusca
    {
        public Index Indice { get; set; }

        public List<Filtro> Filtros { get; set; }
        
    }
}