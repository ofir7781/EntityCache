using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Threading;

namespace EntityCache
{
    public class Program 
    {
        static void Main(string[] args)
        {
            EntityCacheConsoleUI<Student> entityCacheConsoleUI = new EntityCacheConsoleUI<Student>();
            entityCacheConsoleUI.Run();
        }
    }
}




