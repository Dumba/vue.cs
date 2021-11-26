using System;
using Microsoft.Extensions.DependencyInjection;
using Vue.cs.Framework.Exceptions;

namespace Vue.cs.Framework.Extensions
{
    public static class IServiceProviderExtension
    {
        public static T Get<T>(this IServiceProvider self)
        {
            var service = self.GetService<T>()
                ?? throw new NotRegisteredException(nameof(T));

            return service;
        }
    }
}