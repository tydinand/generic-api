# Generic API .net 5

###### This Generic API is created out of the original version from https://github.com/matjazmav/generic-api

* The basic application is a default web-api project in VS. *

>- The generic repository is changed with a more worked-out version. 

>- GenricController as a function GetPosts. 
This specific funtion is now available on all repositories.

>- Added post controller to show how to extend one (post) repository with specific funtions. 

## Create a Generic API
Create a new project in VS (Apsnet core web application => name => Asp.net core web API => create).\
Build and test. See swagger page with get/WeatherForecast and see the results after execute.

Add from nuget: Microsoft.EntityFrameworkCore

Copy into your project:
- 3 files (GenericControllerExtensions.cs, GenericControllerAttribute.cs, GenericControllerApplicationPart.cs from Extensions
- repository.cs from Models/Repository and ApplicationEntity.cs from Models

- Create models (or take from this github) and let them inherrit from ApplicationEntity
- Create a applicationDBContext (or take from this github) and add every modelclass as property. Example 

`code` 

        public DbSet<Post> Posts { get; set; }
        
`code`

- Take care of the reference errors

*ApplicationEntity.cs is a common class of properties that can be used in your generic-repository as property. At least a field like Id must be in it*

Add in Startup.cs in section ConfigureServices above addswaggerGen:

-  services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

-  services.AddControllers().AddGenericControllers<ApplicationDbContext, ApplicationEntity>(typeof(GenericController<>));

Add your model classes (or take from this github).\
Create a new api controller.
- Change the classline into 

`code` 

        public class GenericController<T> : Controller where T : ApplicationEntity 

`code`
- Add above the contructor 

`code` 

        private readonly IRepository<T> repository; 

`code`
- Change the contructor into

`code` 

        public GenericController(IRepository<T> repository)        
        {
            this.repository = repository;
        }
        
`code`

Add a function into this controller like

`code`

        [HttpGet("{Id}")]
        public async Task<T> Get(int Id) 
        {
            return await repository.GetById(Id);
        }

`code`

Fix all references and test your code. 

You should see all modelclasses named each with the functions from the GenericController.\
In the example all modelclasses should have the function Get(int Id).

## Add specific function for each modelclass
If you want every ModelClass to have a specific function you can add this function into the genericController for example.

`code`

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
        
`code`

You will be able to call GetPosts/{Id} from every api-repository call. 

## Extend one Repository with functions

Functions only available for one repository must be set in there own repository (like PostController.cs in the example code).\
Be aware to have different functionNames and paths then already used or generated by the generic-Controller. 

The GetNewPostbyId function is only available in the post repository.

## Create a Repository
- Add a new controller into your project with the name of your model ({ModelName}Controller.cs.
- Let the controller inherrit from controllerBase insteat of controller.
- Leave 

`code` 

        [ApiController]
        [Route("[controller]")] 

`code` 

as it is.
- Reference to a Model can be directly to the generic repository. You even benefit from all the function in it and don't need to register the Repository in the startup. 

 `code` 
 
        IRepository<{Modelname}> repository 
 
 `code`
 
- Add 

`code`

        [HttpGet("{Functionname}/{{Parameters}}"] 

`code` 

(only required parameter must be asked between {}).
- Add Function
- By returning you maybe need to Json it. See example in the post controller. 

Keep coding.

Dinand
