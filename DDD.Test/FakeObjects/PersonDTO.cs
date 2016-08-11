using RaraAvis.nCubed.DDD.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.DDD.Test.FakeObjects
{
    public class PersonDTO : EntityDto
    {
        public PersonDTO() { }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Age { get; set; }
    }
}
