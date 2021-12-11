﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Command : IEntity
    {
        public Command()
        {
            Payments = new HashSet<Payment>();
            //Places = new HashSet<Place>();
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public String AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        //public virtual ICollection<Place> Places { get; set; }
    }
}
