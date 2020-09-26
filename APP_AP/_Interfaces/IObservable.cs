using MDL_AP.Models.ActionPlan;
using MDL_BASE.Interfaces;

namespace MDL_AP.Interfaces
{
   public interface IObservable
    {
        void RegisterObserver(IModelEntity ob, int actionId, string userId, ObserverType observerType);
        void DeleteObserver(IModelEntity ob, int actionId, string userId, ObserverType observerType);
    }
}
