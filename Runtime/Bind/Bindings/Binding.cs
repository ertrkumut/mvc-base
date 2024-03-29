﻿using System.Collections.Generic;

namespace MVC.Runtime.Bind.Bindings
{
    public class Binding : IBinding
    {
        public object Key { get; protected set; }
        public object Value { get; protected set; }

        public virtual void SetKey(object key)
        {
            Key = key;
        }

        public virtual void SetValue(object value)
        {
            Value = value;
        }
        
        public virtual void To<TValueType>()
        {
            var values = Value as List<object>;
            if (values == null)
                values = new List<object>();
            
            values.Add(typeof(TValueType));
            Value = values;
        }

        public virtual void Clear()
        {
            Key = null;
            Value = null;
        }
    }
}