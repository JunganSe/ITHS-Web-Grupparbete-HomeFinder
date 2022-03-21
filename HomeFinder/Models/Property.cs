using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.Models
{
    public class Property
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Number of Rooms")]
        public int NumberOfRooms { get; set; }

        [Required]
        [Display(Name = "Building Area")]
        public double BuildingArea { get; set; }

        [Required]
        [Display(Name = "Plot Area")]
        public double PlotArea { get; set; }

        [Required]
        [Display(Name = "Construction Year")]
        public int ConstructionYear { get; set; }

        [Required]
        [Display(Name = "Number of Views")]
        public int NumberOfViews { get; set; }

        [Required]
        [Display(Name = "Map Url")]
        public string MapUrl { get; set; }

        [Required]
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; }

        [Display(Name = "Viewing Date")]
        public DateTime? ViewingDate { get; set; }


        // Foreign keys
        public int AdressId { get; set; }
        public Adress Adress { get; set; } // 1 to 1

        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; } // 1 to many

        public int TenureId { get; set; }
        public Tenure Tenure { get; set; } // 1 to many

        public int SaleStatusId { get; set; }
        public SaleStatus SaleStatus { get; set; } // 1 to many

        [Required]
        public string EstateAgentId { get; set; }
        public ApplicationUser EstateAgent { get; set; } // 1 to many


        // Koppling
        public ICollection<ExpressionOfInterest> ExpressionOfInterests { get; set; } = new List<ExpressionOfInterest>();
    }
}
