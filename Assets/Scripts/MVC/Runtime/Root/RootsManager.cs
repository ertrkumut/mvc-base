namespace MVC.Runtime.Root
{
    public class RootsManager
    {
        private static RootsManager _instance;
        public static RootsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RootsManager();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            
        }
    }
}