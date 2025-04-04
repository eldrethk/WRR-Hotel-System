﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WRRManagement.Domain.Amenities;
using WRRManagement.Domain.Packages;
using WRRManagement.Domain.RoomTypes;

namespace WRR8_0.Models
{
    public class PackageViewModel : IValidatableObject
    {
        public Package Package { get; set; }
        public List<RoomType>? RoomTypes { get; set; }
        public List<ExtraAmenity>? Amenities { get; set; }

        public UploadImageViewModel? UploadImage { get; set; }

        public string PackageType { get; set; }
        [Required(ErrorMessage = "Rooms are required")]
        public int[] SelectedRoomTypeIds { get; set; }

        public IEnumerable<SelectListItem> PackageRoom
        {
            get { return RoomTypes != null ? new SelectList(RoomTypes, "RoomTypeID", "Name") : new List<SelectListItem>(); }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PackageType == "Percentage")
            {
                Package.PercentOff = true;
                if (Package.PercentageOff == null)
                    yield return new ValidationResult("Percentage Off is required");
            }
            else if(PackageType == "Nights")
            {
                Package.NightsFree = true;
                if (Package.NumberOfNights == null || Package.NumberOfNights <= 0)
                    yield return new ValidationResult("Nights free is required");
            }
        }
    }
    
}
