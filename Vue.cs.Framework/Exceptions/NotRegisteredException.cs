using System;
using System.Runtime.Serialization;

namespace Vue.cs.Framework.Exceptions
{
    [Serializable]
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException(string missingType) : base("Type is not registered in DependecyInjection. Please run IServiceCollection.AddVueCs() on initialize.")
        {
            MissingType = missingType;
        }

        public string MissingType { get; set; }
    }
}