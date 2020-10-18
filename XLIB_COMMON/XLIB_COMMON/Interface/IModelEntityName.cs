using MDLX_CORE.Interfaces;

namespace MDLX_CORE.Interface
{
    public interface IModelEntityName : IModelEntity
    {
        string Name { get; set; }
    }
}
