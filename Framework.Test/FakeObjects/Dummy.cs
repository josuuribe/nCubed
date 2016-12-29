using System.ComponentModel.Composition;

namespace RaraAvis.nCubed.Core.Test.FakeObjects
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
