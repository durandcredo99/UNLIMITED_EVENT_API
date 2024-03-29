﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class PlaceRequest
    {
        public Guid? Id { get; set; }
        [Display(Name = "Numéro de place")]
        public int NoPlace { get; set; }
        [Display(Name = "Prix")]
        public long Price { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string Status { get; set; }

        [Required]
        [Display(Name = "Nbr de Places à ajouter")]
        public int NbrPlace { get; set; }

        [Required]
        public Guid EventId { get; set; }

        public Guid? OrderId { get; set; }
    }
}
