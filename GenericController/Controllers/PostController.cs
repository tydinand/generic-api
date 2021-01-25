using GenericController.API.Models;
using GenericController.API.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepository<Post> repository;

        public PostController(ILogger<WeatherForecastController> logger, IRepository<Post> repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        // Functions in this class will only available for post class requests.
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetNewPostbyId/{id}")]
        public Post GetNewPostbyId(int id)
        {
            // return example code use repository. (comment out by the lack of a Database)
            //return repository.GetById(id);

            //Return example code json (nuget newtonsoft.json and add {using newtonsoft.json}
            //var result = repository.GetById(id);
            //JsonConvert.SerializeObject(result);
            return new Post();
        }
    }
}
