﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Domain;

namespace U.ProductService.Application.Products.Commands.Update
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UpdateProductCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(
            ILogger<UpdateProductCommandHandler> logger,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (command.Product is null)
            {
                command.Product = await _productRepository.GetAsync(command.ProductId);
            }

            if (command.Product is null)
            {
                _logger.LogInformation($"Product with id: '{command.ProductId}' has been not found");
                throw new ProductNotFoundException($"Product with id: '{command.ProductId}' has not been found");
            }

            var dimensions = GetDimensions(command);

            command.Product.UpdateProduct(_mapper,
                command.Name,
                command.Description,
                command.Price,
                dimensions,
                DateTime.UtcNow);

            _productRepository.Update(command.Product);

            await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }

        private Dimensions GetDimensions(UpdateProductCommand command)
        {
            return new Dimensions(command.Dimensions.Length,
                command.Dimensions.Width,
                command.Dimensions.Height,
                command.Dimensions.Weight);
        }
    }
}