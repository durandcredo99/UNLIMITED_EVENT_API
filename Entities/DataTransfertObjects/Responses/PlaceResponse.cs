using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PlaceResponse
    {
        public Guid Id { get; set; }
        public int NoPlace { get; set; }
        public float Price { get; set; }

        public Guid EventId { get; set; }
        public EventResponse Event { get; set; }
        public Guid CommandId { get; set; }
        public CommandResponse Command { get; set; }
    }
}
