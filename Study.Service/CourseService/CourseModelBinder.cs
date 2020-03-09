using Microsoft.AspNetCore.Mvc.ModelBinding;
using Study.Services.CourseService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Study.Service.CourseService
{
    class CourseModelBinder : IModelBinder
    {
        private readonly ICourseService courseService;

        public CourseModelBinder(Services.CourseService.ICourseService courseService)
        {
            this.courseService = courseService;
        }
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return;
                //return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            //if (!int.TryParse(value, out var id))
            //{
            //    // Non-integer arguments result in model state errors
            //    bindingContext.ModelState.TryAddModelError(
            //        modelName, "Author Id must be an integer.");

            //    return Task.CompletedTask;
            //}

            var courseDto = await courseService.GetCourseByIdAsync(value);
            if(courseDto != null)
            {
                bindingContext.Result = ModelBindingResult.Success(courseDto);
            }
           
            // Model will be null if not found, including for
            // out of range id values (0, -3, etc.)
            //var model = _context.Authors.Find(id);
            //bindingContext.Result = ModelBindingResult.Success(model);

            //return Task.CompletedTask;
        }
    }
}
