﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using U.Common.NetCore.Cache;
using U.ProductService.Domain;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Contexts;

namespace U.ProductService.Persistance.Repositories.Manufacturer
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ProductContext _context;
        private readonly ICachingRepository _cachingRepository;
        public IUnitOfWork UnitOfWork => _context;

        public ManufacturerRepository(ProductContext context, ICachingRepository cachingRepository)
        {
            _context = context;
            _cachingRepository = cachingRepository;
        }

        public async Task<Domain.Entities.Manufacturer.Manufacturer> AddAsync(Domain.Entities.Manufacturer.Manufacturer manufacturer)
        {
            return (await _context.Manufacturers.AddAsync(manufacturer)).Entity;
        }

        public async Task AddProductAsync(Domain.Entities.Picture.Picture picture)
        {
            await _context.Pictures.AddAsync(picture);
        }

        public async Task<Domain.Entities.Manufacturer.Manufacturer> GetAsync(Guid manufacturerId)
        {
            var cached = _cachingRepository.Get<Domain.Entities.Manufacturer.Manufacturer>(manufacturerId.ToString());

            if (cached != null)
            {
                return cached;
            }

            var manufacturer = await _context.Manufacturers
                .Include(x => x.Pictures)
                .FirstOrDefaultAsync(x => x.Id.Equals(manufacturerId));

            if (manufacturer != null)
            {
                _cachingRepository.Cache(manufacturerId.ToString(), manufacturer);
            }

            return manufacturer;
        }

        public async Task<IList<Domain.Entities.Manufacturer.Manufacturer>> GetManyAsync()
        {
            var slug = "allManufacturers";
            var cached = _cachingRepository.Get<List<Domain.Entities.Manufacturer.Manufacturer>>(slug);

            if (cached != null)
            {
                return cached;
            }

            var manufacturers = await _context.Manufacturers
                .ToListAsync();

            if (manufacturers != null && manufacturers.Any())
            {
                _cachingRepository.Cache(slug, manufacturers);
            }

            return manufacturers;
        }

        public async Task<Domain.Entities.Manufacturer.Manufacturer> GetUniqueClientIdAsync(string uniqueClientId)
        {
            var cached = _cachingRepository.Get<Domain.Entities.Manufacturer.Manufacturer>(uniqueClientId);

            if (cached != null)
            {
                return cached;
            }

            var manufacturer =
                await _context
                    .Manufacturers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UniqueClientId.Equals(uniqueClientId));

            if (manufacturer != null)
            {
                _cachingRepository.Cache(uniqueClientId, manufacturer);
            }

            return manufacturer;
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            var cached = _cachingRepository.Get<Domain.Entities.Manufacturer.Manufacturer>($"Manufacturer_AsNoTracking_{id}");

            if (cached != null)
            {
                return true;
            }

            var manufacturer = await _context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

           if (manufacturer != null)
           {
               _cachingRepository.Cache(id.ToString(), manufacturer);
           }

            return manufacturer != null;
        }

        public void Update(Domain.Entities.Product.Product product)
        {
            _context.SingleUpdateAsync(product);
        }
    }
}