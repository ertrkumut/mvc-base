namespace MVC.Pool
{
    public interface IPoolable
    {
        void OnGetFromPool();
        void OnReturnToPool();
    }
}