using System;

namespace WebAPI.ViewModels.Property
{
    public class PropertyViewModel
    {

        public int PropertyId { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public int NumberOfRooms { get; set; }
        public double BuildingArea { get; set; }
        public double BeeArea { get; set; }
        public double PlotArea { get; set; }
        public int ConstructionYear { get; set; }
        public int NumberOfViews { get; set; }
        public string MapUrl { get; set; }
        public string Address { get; set; }
        public string EstateAgent { get; set; }
        public string PropertyType { get; set; }
        public string SaleStatus { get; set; }
        public string Tenure { get; set; }
        public DateTime PublishingDate { get; set; }
        public DateTime? ViewingDate { get; set; }

    }
}