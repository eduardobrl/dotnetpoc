using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataFilters;
using dotnetPoc.Domain;
using dotnetPoc.Domain.Dto;
using dotnetPoc.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace dotnetPoc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDynamoRepo _dynamoRepo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
        IDynamoRepo dynamoRepo)
        {
            _logger = logger;
            _dynamoRepo = dynamoRepo;
        }

        [HttpGet]
        public ParametrosDeBusca Get([FromQuery] FilterParamsRequest param)
        {
            var proplist = param.GetType().GetProperties().Select(x => x.Name).ToList<string>();

            List<Filter> filters = new List<Filter>();

            return this._dynamoRepo.ObterComFiltros(new FiltersParamsInput {
                Cnpj = param.Cnpj,
                Date = param.Date,
                Name = param.Name,
                Produto = param.Produto,
                Status = param.Status,
                Valor = Convert.ToDouble(param.Valor)

            });

        }

        private void IsIndex()
        {

        }
    }
}
