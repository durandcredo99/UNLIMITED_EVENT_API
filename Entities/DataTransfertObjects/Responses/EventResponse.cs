using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public class EventResponse : IComparable<EventResponse>
    {
        public Guid Id { get; set; }
        public string ImgLink { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        public string Description { get; set; }
        [BindProperty, DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Display(Name = "Prix")]
        public long Price { get; set; }
        [Display(Name = "Statut de l'événement")]
        public string IsPublic { get; set; }
        [Display(Name = "Statut")]
        public string StatusEvent { get; set; }
        [Display(Name = "Places")]
        public int NbrPlace { get; set; }
        public string Lieu { get; set; }
        [Display(Name = "Favori")]
        public int NoPriority { get; set; }

        [Display(Name = "Catégorie")]
        public Guid CategoryId { get; set; }
        public CategoryResponse Category { get; set; }

        public String AppUserId { get; set; }
        public AppUserResponse AppUser { get; set; }
        [Display(Name = "Sponsor")]
        public Guid SponsorId { get; set; }
        public SponsorResponse Sponsor { get; set; }

        public virtual ICollection<PlaceResponse> Places { get; set; }

        public int CompareTo(EventResponse otherEventResponse)
        {
            if (Id == otherEventResponse.Id)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
