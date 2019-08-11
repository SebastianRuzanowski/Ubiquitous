﻿using System;
using System.Collections.Generic;
using System.Linq;
using U.ProductService.Domain.Events;
using U.ProductService.Domain.Exceptions;
using U.ProductService.Domain.SeedWork;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable CheckNamespace


namespace U.ProductService.Domain.Aggregates
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string BarCode { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? LastFullUpdateDateTime { get; private set; }

        //value object
        public Dimensions Dimensions { get; private set; }

        public Guid ManufacturerId { get; private set; }

        public ICollection<Picture> Pictures { get; set; }

        private Product()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            BarCode = string.Empty;
            Description = string.Empty;
            IsPublished = default;
            CreatedDateTime = DateTime.UtcNow;
            LastFullUpdateDateTime = default;
            IsPublished = default;
        }

        public Product(string name, decimal price, string barCode, string description, Dimensions dimensions,
            Guid manufacturerId) : this()
        {
            Name = name;
            Price = price;
            BarCode = barCode;
            Description = description;
            Dimensions = dimensions;
            ManufacturerId = manufacturerId;

            var @event = new ProductAddedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        public bool CompareAlternateId(string productUniqueCode) => BarCode.Equals(productUniqueCode);

        public void AddPicture(Guid id, string seoFilename, string description, string url, string mimeType)
        {
            if (string.IsNullOrEmpty(seoFilename))
                throw new ProductDomainException($"{nameof(seoFilename)} cannot be null or empty!");

            if (string.IsNullOrEmpty(description))
                throw new ProductDomainException($"{nameof(description)} cannot be null or empty!");

            if (string.IsNullOrEmpty(url))
                throw new ProductDomainException($"{nameof(url)} cannot be null or empty!");

            if (string.IsNullOrEmpty(mimeType))
                throw new ProductDomainException($"{nameof(mimeType)} cannot be null or empty!");

            var picture = new Picture(id, seoFilename, description, url, mimeType);
            
            Pictures.Add(picture);

            var @event = new ProductPictureAddedDomainEvent(Id, picture.Id, seoFilename);

            AddDomainEvent(@event);
        }

        public void DeletePicture(Guid pictureId)
        {
            var picture = Pictures.FirstOrDefault(x => x.Id.Equals(pictureId));

            if (picture is null)
                throw new ProductDomainException("Picture does not exist!");

            Pictures.Remove(picture);

            var @event = new ProductPictureRemovedDomainEvent(Id, picture.Id);

            AddDomainEvent(@event);
        }

        public void ChangePrice(decimal price)
        {
            if (price < 0)
                throw new ProductDomainException("Price cannot be below 0!");

            var previousPrice = Price;

            Price = price;

            var @event = new ProductPriceChangedDomainEvent(Id, previousPrice, Price);

            AddDomainEvent(@event);
        }

        public void Publish()
        {
            if (IsPublished) return;
            IsPublished = true;

            var @event = new ProductPublishedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        public void UnPublish()
        {
            if (IsPublished == false) return;

            IsPublished = false;

            var @event = new ProductUnpublishedDomainEvent(Id, Name, Price, ManufacturerId);

            AddDomainEvent(@event);
        }

        public void UpdateAllProperties(string name, decimal price, Dimensions dimensions, DateTime updateGenerated)
        {
            if (LastFullUpdateDateTime < updateGenerated)
            {
                //avoiding any lagged-in-time updates 
                Name = name;
                Price = price;
                Dimensions.Height = dimensions.Height;
                Dimensions.Length = dimensions.Length;
                Dimensions.Weight = dimensions.Weight;
                Dimensions.Width = dimensions.Width;

                var @event = new ProductPropertiesChangedDomainEvent(Id, Name, Price);
                AddDomainEvent(@event);
            }

            // add new update saying event has been raised after last up-to-date update
        }
    }
}