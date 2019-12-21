using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCache
{
    // This class will hold Dictionary of Schools that will be our users in this example.
    // each School is a User in the program and can use the entityCache functions.
    public class SchoolsManager<T> where T : Entity, new()
    {       

        private readonly Dictionary<int, School<T>> r_Schools;

        public SchoolsManager()
        {
            r_Schools = new Dictionary<int, School<T>>();
        }

        public Dictionary<int, School<T>> Schools
        {
            get { return r_Schools; }
        }

        public bool IsSchoolExists(int i_KeyNumber)
        {
            return r_Schools.ContainsKey(i_KeyNumber);
        }

        public School<T> GetRequestedSchool(int i_KeyNumber)
        {
            School<T> res = r_Schools[i_KeyNumber];
            return res;
        }
    }
}
