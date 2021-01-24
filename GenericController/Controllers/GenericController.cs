using GenericController.API.Extensions;
using GenericController.API.Models;
using GenericController.API.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace GenericController.API.Controllers
{
    [GenericController]
    [Route("[controller]")]
    public class GenericController<T> : Controller where T : ApplicationEntity
    {
        private readonly ILogger<GenericController<T>> _logger;
        private readonly IRepository<T> repository;
        private readonly IRepository<Post> postRepository;

        public GenericController(ILogger<GenericController<T>> logger, IRepository<T> repository, IRepository<Post> postRepository)
        {
            _logger = logger;
            this.repository = repository;
            this.postRepository = postRepository;
        }

        [HttpGet("{Id}")]
        public async Task<T> Get(int Id) 
        {
            return await repository.GetById(Id);
        }

        //Example of function for every class. 
        //To have it only available for a specific class use a seperate controller like PostController

        [HttpGet("GetPosts/{Id}")]
        public async Task<string> GetPosts(int Id, string search= null, bool active = false, bool includsComments = false)
        {
           var result = (await postRepository.GetAllInclude(x => x.Include(p => p.Comments)))
                    .Where(x => (includsComments ? x.Comments?.Count > 0 : x.Id != 1) && (search != null ? x.Title.ToLower().Contains(search.ToLower()) : x.Id != -1)).ToList();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
            return JsonConvert.SerializeObject(result);
        }
    }
}
