using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCache
{
    public class School<T> where T : Entity, new()
    {
        private int m_Id;
        private EntityCache<T> m_SchoolEntityCache;
        private IRepositoryProvider<T> m_repositoryProvider;

        public School(int i_Id, string i_RepositoryName, EntityCache<T>.eInitializeState i_InitializeState)
        {
            m_Id = i_Id;
            m_repositoryProvider = new TextFileRepositoryProvider<T>(i_RepositoryName);
            m_SchoolEntityCache = new EntityCache<T>(i_InitializeState, m_repositoryProvider);
            m_SchoolEntityCache.EventProperty += new ReportUserActionDelegate(this.ReportAction);
        }



        public int id
        {
            get { return m_Id; }
        }

        public EntityCache<T> SchoolEntityCache
        {
            get { return m_SchoolEntityCache; }
        }
        public T GetCachedEntity(int i_EntityId)
        {
            return m_SchoolEntityCache.GetCachedEntity(i_EntityId);
        }

        public void AddNewEntity(int i_EntityId, T i_Entity)
        {
            m_SchoolEntityCache.AddNewEntity(i_EntityId, i_Entity);
        }

        public void RemoveCachedEntity(int i_EntityId)
        {
            m_SchoolEntityCache.RemoveCachedEntity(i_EntityId);
        }

        public void UpdateCachedEntity(int i_EntityId, T i_Entity)
        {
            m_SchoolEntityCache.UpdateCachedEntity(i_EntityId, i_Entity);
        }

        public void ReportAction(string i_ActionMsg, int i_EntityId)
        {
            Console.WriteLine("{0} Entity id number: {1} was performed successfully.", i_ActionMsg, i_EntityId);
        }
    }
}
