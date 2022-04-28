namespace MVC.Runtime.Pool
{
    public interface IPoolable
    {
        void OnGetFromPool();
        void OnReturnToPool();
    }
}