using RaraAvis.nCubed.DDD.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Test.FakeObjects
{
    [Export]
    public class Person : Entity, IComparable, IAggregateRoot
    {
        public Person(Guid id) : base(id) { }
        [ImportingConstructor]
        public Person()  {
            this.AggregateRoot = this;
        }

        public int Id { get; set; }

        public long IdLong { get; set; }

        public string Name { get; set; }
        public string SurName { get; set; }
        public string Age { get; set; }

        public Guid AggregateId
        {
            get
            {
                return this.EntityId;
            }
        }

        public int CompareTo(object obj)
        {
            Person p = obj as Person;
            return String.Compare(p.Name, this.Name);
        }
    }
}
