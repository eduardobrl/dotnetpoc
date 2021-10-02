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

        public ParametrosDeBusca ObterComFiltros(FiltersParamsInput filtros)
        {
            var listaDeFiltros = ListarFiltros(filtros);
            var parametrosDeBusca = ObterParametrosDeBusca(listaDeFiltros);

            return parametrosDeBusca;

        }

        private List<Filtro> ListarFiltros(FiltersParamsInput input)
        {
            List<Filtro> filtros = new List<Filtro>();

            var proplist = input.GetType().GetProperties().Select(x => x.Name).ToList<string>();
            

            foreach (var propname in proplist)
            {
                var value = input.GetType().GetProperty(propname).GetValue(input);
                var tablepropname = typeof(OperacaoDb).GetProperty(propname).GetCustomAttribute<DynamoDbPropertyAttribute>()?.Value;
                var nomeindice = typeof(OperacaoDb).GetProperty(propname).GetCustomAttribute<DynamoDbIndexAttribute>()?.Value;
                var nomerange = typeof(OperacaoDb).GetProperty(propname).GetCustomAttribute<DynamoDbRangeAttribute>()?.Value;


                if(value != null)
                {                
                    filtros.Add(new Filtro(propname, tablepropname, value, nomeindice, nomerange));
                }
 
            }

            return filtros;
        }


        private ParametrosDeBusca ObterParametrosDeBusca(List<Filtro> ListaDeFitros)
        {
            List<Index> indices = new List<Index>();
            List<Filtro> filtros = new List<Filtro>();

            foreach (var filtro in ListaDeFitros)
            {
                if(filtro.IndicesNomePk != null && filtro.Operacao == FilterOperator.EqualTo)
                {
                    foreach (var ind in filtro.IndicesNomePk)
                    {
                        var filtroSk = ObterRangeKeyParaIndice(ListaDeFitros, ind);

                        indices.Add(new Index {
                            IndexName = ind,
                            Pk = filtro,
                            Sk = filtroSk
                        });
                    }

                }
             
            }

            var indice = ObterIndicePreferencial(indices);


            ListaDeFitros.Remove(indice.Pk);
            ListaDeFitros.Remove(indice.Sk);

            return new ParametrosDeBusca {
                Filtros = ListaDeFitros,
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

        private Filtro ObterRangeKeyParaIndice(List<Filtro> filtros, string indicePk)
        {
            foreach (var filtro in filtros)
            {
                if(filtro.IndicesNomeSk == null)
                    continue;

                foreach (var indicesSk in filtro.IndicesNomeSk)
                {
                    if(indicesSk == indicePk)
                        return filtro;
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
        public string EntityProp { get; set; }

        public string TableProp { get; set; }

        public FilterOperator Operacao { get; set; }

        public object Valor { get; set; }

        public string[] IndicesNomePk { get; set; }

        public string[] IndicesNomeSk { get; set; }

        public Filtro(string entityprop, string dynamoprop, object valor, string[] indicesNomePk, string[] indicesNomeSk, FilterOperator op = FilterOperator.EqualTo)
        {
            EntityProp = entityprop;
            TableProp = dynamoprop;
            Valor = valor;
            Operacao = op;
            IndicesNomePk = indicesNomePk;
            IndicesNomeSk = indicesNomeSk;
        }
    }

    public class ParametrosDeBusca
    {
        public Index Indice { get; set; }

        public List<Filtro> Filtros { get; set; }
        
    }
}