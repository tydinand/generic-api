using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace GenericController.API.Extensions
{
    //ref: https://github.com/matjazmav/generic-api
    public class GenericControllerAttribute : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var entityType = controller.ControllerType.GetGenericArguments()[0];

            controller.ControllerName = entityType.Name;
        }
    }
}
