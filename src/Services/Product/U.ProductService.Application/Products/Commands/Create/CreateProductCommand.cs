﻿using System;
using MediatR;
using Newtonsoft.Json;
using U.ProductService.Application.Products.Models;

namespace U.ProductService.Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get;  set; }
        public string BarCode { get;  set; }
        public decimal Price { get;  set; }
        public string Description { get;  set; }
        public Guid? CategoryId { get; set; }
        public DimensionsDto Dimensions { get;  set; }

        [JsonConstructor]
        public CreateProductCommand()
        {
                
        }

        public CreateProductCommand(string name, string barCode, decimal price, string description, DimensionsDto dimensions, Guid? categoryId = null)
        {
            Name = name;
            BarCode = barCode;
            Price = price;
            Description = description;
            CategoryId = categoryId;
            Dimensions = dimensions;
        }
    }
}