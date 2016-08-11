using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.Core.Configurations.Common.Types
{
    /// <summary>
    /// Elements to be excluded for MEF.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "It is used by Configuration Infrastructure"), ConfigurationCollection(typeof(ExcludeElement), AddItemName = "type", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ExcludeCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Override for CreateNewElement()
        /// </summary>
        /// <returns>A ConfigurationElement object.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExcludeElement();
        }

        /// <summary>
        /// Override for GetElementKey()
        /// </summary>
        /// <param name="element">A configuration element object.</param>
        /// <returns>An object with configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExcludeElement)element).Type;
        }
    }
}
