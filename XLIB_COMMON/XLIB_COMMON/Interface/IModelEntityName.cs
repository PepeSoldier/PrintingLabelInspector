using MDL_BASE.Interfaces;

namespace MDL_BASE.Interface
{
    public interface IModelEntityName : IModelEntity
    {
        string Name { get; set; }
    }
}
