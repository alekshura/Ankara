namespace Compentio.Ankara.Api.Providers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class ArrayParameterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.HttpContext.Request.Query[bindingContext.FieldName];

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            var ints = value.ToString().Split(',').Select(long.Parse).ToArray();

            bindingContext.Result = ModelBindingResult.Success(ints);

            if (bindingContext.ModelType == typeof(IEnumerable<long>))
            {
                bindingContext.Result = ModelBindingResult.Success(ints.ToList());
            }

            return Task.CompletedTask;
        }
    }
}