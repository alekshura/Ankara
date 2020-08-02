namespace Compentio.Ankara.Api.Providers
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
    using System;
    using System.Collections.Generic;

    public class ArrayBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.BindingInfo.BindingSource != BindingSource.Query)
            {
                return null;
            }

            if (context.Metadata.ModelType == typeof(long[]) || context.Metadata.ModelType == typeof(IEnumerable<long>) ||
                context.Metadata.ModelType == typeof(IList<long>))
            {
                return new BinderTypeModelBinder(typeof(ArrayParameterBinder));
            }

            return null;
        }
    }
}
