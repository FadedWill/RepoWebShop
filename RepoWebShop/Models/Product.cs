﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RepoWebShop.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string Name { get; set; }
        public decimal? OldPrice { get; set; }

        [Display(Name = "Precio")]
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Display(Name = "Categoría")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Temperatura")]
        public string Temperature { get; set; }

        [Display(Name = "Cantidad mínima")]
        [Required]
        [Range(1, int.MaxValue)]
        public int MinOrderAmount { get; set; }

        public bool IsActive { get; set; }
    }
}
