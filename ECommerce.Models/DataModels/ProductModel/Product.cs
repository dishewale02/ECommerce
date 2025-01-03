﻿
using ECommerce.Models.DataModels.InfoModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.DataModels.ProductModel
{
    public class Product: GenericInfo
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? ImagePaths { get; set; }
    }
}
