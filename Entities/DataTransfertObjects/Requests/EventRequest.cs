using Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class EventRequest
    {
        public Guid? Id { get; set; }
        public string ImgLink { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public string StatusEvent { get; set; }
        [Required]
        public int NoTicket { get; set; }
        public int NoPriority { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public String UsersId { get; set; }

        public Guid SponsorId { get; set; }
        public IFormFile File { get; set; }
    }
}
