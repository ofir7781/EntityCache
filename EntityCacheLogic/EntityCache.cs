using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCache
{
    // Adding listeners to our EntityCache.
    public delegate void ReportUserActionDelegate(string i_ActionMsg, int i_EntityId);

    public class EntityCache<T> where T : Entity
    {

        public enum eInitializeState
        {
            EagerLoading = 0,
            LazyLoading = 1,         
        }

        private const string k_AddingEntity = "Adding";
        private const string k_removingEntity = "Removing";
        private const string k_UpdatingEntity = "Updating";

        private event ReportUserActionDelegate i_ReportUserAction;
        private Dictionary<int, T> m_EntityCache;
        private IRepositoryProvider<T> m_EntityUserRepository;

        public EntityCache(eInitializeState i_InitializeState, IRepositoryProvider<T> i_EntityUserRepository)
        {
            m_EntityCache = new Dictionary<int, T>();
            m_EntityUserRepository = i_EntityUserRepository;
            switch(i_InitializeState)
            {
                case eInitializeState.EagerLoading:
                    initializeEagerLoading();
                    break;
                case eInitializeState.LazyLoading:
                    break;
                default:
                    break;
            }

        }

        public Dictionary<int, T> entityCache
        {
            get { return m_EntityCache; }
        }

        // Adding or removing a listener
        public event ReportUserActionDelegate EventProperty
        {
            add
            {
                i_ReportUserAction += value;
            }
            remove
            {
                i_ReportUserAction -= value;
            }
        }

        private void initializeEagerLoading()
        {
            try
            {
                m_EntityCache = m_EntityUserRepository.ReadAllExistingEntityFromRepository();
            }
            catch(Exception)
            {
                throw new Exception("cannot read existing entities from repository");
            }
            
        }


        public T GetCachedEntity(int i_EntityId)
        {
            T CachedEntity;

            if(m_EntityCache.ContainsKey(i_EntityId)) // First look for the entity in cache in memory.
            {
                m_EntityCache.TryGetValue(i_EntityId, out CachedEntity); 
            }
            else
            {
                throw new Exception();
            }

            return CachedEntity;
        }
    
         public void AddNewEntity(int i_EntityId, T i_Entity)
        {
            if(!m_EntityCache.ContainsKey(i_EntityId)) // First look there is already same entity in cache in memory.
            {
                m_EntityCache.Add(i_EntityId, i_Entity);
                m_EntityUserRepository.AddNewEntityToRepository(i_EntityId, i_Entity); 
                notifyListenersAboutAction(k_AddingEntity, i_EntityId);
            }
            else
            {
                throw new Exception();
            }

        }

        public void RemoveCachedEntity(int i_EntityId)
        {
            if(m_EntityCache.ContainsKey(i_EntityId)) // First look for existence of the entity in cache in memory.
            {
                m_EntityCache.Remove(i_EntityId);
                m_EntityUserRepository.RemoveEntityFromRepository(i_EntityId); 
                notifyListenersAboutAction(k_removingEntity, i_EntityId);
            }
            else
            {
                throw new Exception("There is no Entity with this id");
            }

        }

        public void UpdateCachedEntity(int i_EntityId, T i_Entity)
        {
            if (m_EntityCache.ContainsKey(i_EntityId)) // First look for existence of the entity in cache in memory.
            {
                m_EntityCache.Remove(i_EntityId);
                m_EntityCache.Add(i_EntityId, i_Entity);
                m_EntityUserRepository.UpdateEntityInRepository(i_EntityId, i_Entity); 
                notifyListenersAboutAction(k_UpdatingEntity, i_EntityId);
            }
            else
            {
                throw new Exception();
            }
        }

        private void notifyListenersAboutAction(string i_ActionMsg, int i_EntityId)
        {
            if(i_ReportUserAction != null)
            {
                i_ReportUserAction.Invoke(i_ActionMsg, i_EntityId);
            }
        }


    }
}
