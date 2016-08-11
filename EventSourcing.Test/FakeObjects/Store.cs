using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaraAvis.nCubed.EventSourcing.Test.FakeObjects
{
    static class Store
    {
        public static Guid Id { get; set; }

        public static string EnvelopeId { get; set; }

        public static string Message { get; set; }
    }
}
