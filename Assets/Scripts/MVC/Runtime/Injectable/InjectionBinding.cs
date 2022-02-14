using MVC.Runtime.Bind.Bindings;

namespace MVC.Runtime.Injectable
{
    public class InjectionBinding : IBinding
    {
        public string Name;
        public object Key { get; set; }
        public object Value { get; set; }
        
        public void SetKey(object key)
        {
            Key = key;
        }

        public void To<TValueType>()
        {
            
        }
    }
}