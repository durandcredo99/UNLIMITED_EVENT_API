using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class CommentRequest
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }

        [Required]
        public Guid BlogId { get; set; }

    }
}
