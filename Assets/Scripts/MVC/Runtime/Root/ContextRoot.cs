using MVC.Runtime.Contexts;
using UnityEngine;

namespace MVC.Runtime.Root
{
    public class ContextRoot<TContextType> : MonoBehaviour 
        where TContextType : IContext, new()
    {
        public int initializeOrder;
        
        protected TContextType _context;

        private void Awake()
        {
            if (_context == null)
                InitializeContext();
        }

        private void Start()
        {
            _context.Launch();
        }

        private void InitializeContext()
        {
            BeforeCreateContext();
            
            _context = new TContextType();
            _context.Initialize(gameObject);
            AfterCreateBeforeStartContext();

            _context.Start();
            
            AfterStarBeforeLaunchContext();
        }

        private void BeforeCreateContext(){}

        private void AfterCreateBeforeStartContext(){}

        private void AfterStarBeforeLaunchContext(){}
    }
}