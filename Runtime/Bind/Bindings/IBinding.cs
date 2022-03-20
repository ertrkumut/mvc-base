namespace MVC.Runtime.Bind.Bindings
{
    public interface IBinding
    {
        public object Key { get; }
        public object Value { get;}

        void SetKey(object key);
        void SetValue(object value);
        
        void To<TValueType>();

        void Clear();
    }
}