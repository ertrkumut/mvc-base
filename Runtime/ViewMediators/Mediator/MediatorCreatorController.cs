using System;
using System.Collections.Generic;

namespace MVC.Runtime.ViewMediators.Mediator
{
    public class MediatorCreatorController
    {
        private Dictionary<Type, List<IMediator>> _pool;

        public MediatorCreatorController()
        {
            _pool = new Dictionary<Type, List<IMediator>>();
        }

        public IMediator GetMediator(Type mediatorType)
        {
            if (!_pool.ContainsKey(mediatorType))
                _pool.Add(mediatorType, new List<IMediator>());

            var mediatorList = _pool[mediatorType];
            var mediator = mediatorList.Count != 0 ? mediatorList[0] : null;

            if (mediator != null)
                _pool[mediatorType].Remove(mediator);
            else
                mediator = (IMediator) Activator.CreateInstance(mediatorType);
            
            return mediator;
        }

        public void ReturnMediatorToPool(IMediator mediator)
        {
            var mediatorType = mediator.GetType();
            
            if(!_pool.ContainsKey(mediatorType))
                _pool.Add(mediatorType, new List<IMediator>());

            var poolList = _pool[mediatorType];
            if(!poolList.Contains(mediator))
                poolList.Add(mediator);
        }
    }
}