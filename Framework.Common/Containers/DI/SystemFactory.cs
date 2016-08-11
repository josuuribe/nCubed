using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Containers.DI
{
    /// <summary>
    /// Class that creates system containers.
    /// </summary>
    /// <typeparam name="T">A factory to create a system container.</typeparam>
    public static class SystemFactory<T> where T : SystemContainer, new()
    {
        private static T container = new T();
        /// <summary>
        /// Container asked.
        /// </summary>
        public static T Container
        {
            get
            {
                return container;
            }
        }
    }
}
