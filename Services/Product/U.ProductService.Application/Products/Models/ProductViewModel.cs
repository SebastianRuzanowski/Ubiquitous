using System;
using System.Collections.Generic;
using Caracan.Templates;
using U.ProductService.Application.Categories.Models;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Models
{
    public class ProductViewModel : ILiquidTemplateObject
    {
        public Guid Id { get; set; }
        public string Name { get;  set; }
        public string BarCode { get;  set; }
        public decimal Price { get;  set; }
        public string Description { get;  set; }
        public bool IsPublished { get;  set; }
        public DateTime CreatedDateTime { get;  set; }
        public DateTime? LastFullUpdateDateTime { get;  set; }
        public Dimensions Dimensions { get;  set; }
        public Guid ManufacturerId { get;  set; }
        public CategoryViewModel Category { get;  set; }
        public IReadOnlyCollection<PictureViewModel> Pictures { get;  set; }
    }
}