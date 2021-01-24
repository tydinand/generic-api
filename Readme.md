# Generic API .net 5

###### This Generic API is created out of the original version from https://github.com/matjazmav/generic-api

*The basic application is a default web-api project in VS. *

>- The generic repository is changed with a more worked-out version. 

>- GenricController as a function GetPosts. 
This specific funtion is now available on all repositories.

>- Added post controller to show how to extend one (post) repository with specific funtions. 

## Create a Generic API
Create a new project in VS (Apsnet core web application => name => Asp.net core web API => create).\
Build and test. See swagger page with get/WeatherForecast and see the results after execute.

Copy into your project:
- 3 files (GenericControllerExtensions.cs, GenericControllerAttribute.cs, GenericControllerApplicationPart.cs from Extensions
- repository.cs from Models/Repository and ApplicationEntity.cs from Models

- Create models (or take from this github) and let them inherrit from ApplicationEntity
- Create a applicationDBContext (or take from this github) and 
- Take care of the reference errors

*ApplicationEntity.cs is a common class of properties that can be used in your generic-repository as property*

Add in Startup.cs in section ConfigureServices above addswaggerGen:
-  services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
-  services.AddControllers().AddGenericControllers<ApplicationDbContext, ApplicationEntity>(typeof(GenericController<>));

Add your model classes (or take from this github).\
Create a new api controller.\
- Change the classline into `code` public class GenericController<T> : Controller where T : ApplicationEntity `code`
- Add above the contructor `code` private readonly IRepository<T> repository; `code`
- Change the contructor into \
`code` public GenericController(IRepository<T> repository)\
        {\
            this.repository = repository;\
        }\
`code`

Fix all references and test your code. 

Good luke with this code example.

Dinand

