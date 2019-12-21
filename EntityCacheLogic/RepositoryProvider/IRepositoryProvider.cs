using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCache
{
    // Every repository provider will have these functionality.
    public interface IRepositoryProvider<T> where T : Entity
    {
        void AddNewEntityToRepository(int i_EntityId, T i_Entity);
        bool RemoveEntityFromRepository(int i_EntityId);
        bool UpdateEntityInRepository(int i_EntityId, T i_Entity);
        Dictionary<int, T> ReadAllExistingEntityFromRepository();

    }
}
