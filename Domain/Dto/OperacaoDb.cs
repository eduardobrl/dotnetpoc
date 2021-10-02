using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetPoc.Domain.Attributes;

namespace dotnetPoc.Domain.Dto
{
    public class OperacaoDb
    {
        public const string GSI1 = "GSI1";
        public const string GSI2 = "GSI2";

        public static Dictionary<string,int> ListaDePrioridades = new Dictionary<string, int>{
                {OperacaoDb.GSI1, 1},
                {OperacaoDb.GSI2, 2}
        };

        [DynamoDbIndex(GSI1)]
        [DynamoDbProperty("cpf_cnpj")]
        public string Cnpj { get; set; }

        [DynamoDbIndex(GSI2)]
        [DynamoDbProperty("data_oper_lqdd")]
        public string Date { get; set; }

        [DynamoDbRange(GSI2)]
        [DynamoDbProperty("idProduto")]
        public string Produto { get; set; }

        [DynamoDbRange(GSI2)]
        [DynamoDbProperty("status_oper_lqdd")]
        public string Status { get; set; }

        [DynamoDbProperty("nom_clie_idt")]
        public string Name { get; set; }


        [DynamoDbProperty("vlr_oper_clie")]
        public double Valor { get; set; }
    }
}