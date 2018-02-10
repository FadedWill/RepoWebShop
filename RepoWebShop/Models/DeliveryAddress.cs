﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RepoWebShop.Models
{
    public class DeliveryAddress
    {
        [StringLength(100)]
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Ingresa una dirección.")]
        public virtual string AddressLine1 { get; set; }

        [StringLength(100)]
        [Display(Name = "Altura")]
        [Required(ErrorMessage = "Ingresa una altura.")]
        public virtual string StreetNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Calle")]
        [Required(ErrorMessage = "Ingresa una calle.")]
        public virtual string StreetName { get; set; }

        [Display(Name = "Codigo Postal")]
        [Required(ErrorMessage = "No pudimos comprobar el código postal. Revisa la dirección.")]
        [StringLength(50, MinimumLength = 4)]
        public virtual string ZipCode { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50)]
        public string State { get; set; }

        [Display(Name = "País")]
        [StringLength(50)]
        public string Country { get; set; }

        [Display(Name = "Distancia")]
        [Range(1, 3000, ErrorMessage = "Nuestra zona de cobertura son 3kms.")]
        public int Distance { get; set; }
    }
}
