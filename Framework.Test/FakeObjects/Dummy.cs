using System.ComponentModel.Composition;

namespace Optimissa.nCubed.Core.Test.FakeObjects
{
    [InheritedExport(typeof(IDummy))]
    public class Dummy : IDummy
    {
        public string Echo
        {
            get; set;
        }
    }
}
