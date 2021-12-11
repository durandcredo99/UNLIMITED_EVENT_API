using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public Guid BlogId { get; set; }
        public BlogResponse Blog { get; set; }

       
    }
}
