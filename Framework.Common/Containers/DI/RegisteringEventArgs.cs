using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Containers.DI
{
    /// <summary>
    /// Class for pre preprocessing objects.
    /// </summary>
    public class RegisteringEventArgs : EventArgs
    {
        private List<Type> types = null;
        private RegistrationBuilder registrationBuilder = null;
        /// <summary>
        /// Base constructor
        /// </summary>
        public RegisteringEventArgs()
        {
            types = new List<Type>();
            registrationBuilder = new RegistrationBuilder();
        }
        /// <summary>
        /// Allows to export a custom type.
        /// </summary>
        /// <typeparam name="T">Type to export.</typeparam>
        public void Export<T>()
        {
            registrationBuilder.ForType<T>().Export();
            types.Add(typeof(T));
        }
        /// <summary>
        /// TypeCatalog with custom types.
        /// </summary>
        public TypeCatalog TypeCatalog
        {
            get
            {
                return new TypeCatalog(types, registrationBuilder);
            }
        }
    }
}
