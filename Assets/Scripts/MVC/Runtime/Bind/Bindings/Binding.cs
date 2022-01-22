using System.Collections.Generic;

namespace MVC.Runtime.Bind.Bindings
{
    public class Binding : IBinding
    {
        public object Key { get; }
        public object Value { get; protected set; }

        public Binding(object key)
        {
            Key = key;
        }

        public void To<TValueType>()
        {
            var values = Value as List<object>;
            values.Add(typeof(TValueType));
            Value = values;
        }
    }
}