﻿using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.Common.Pagination;
using U.ProductService.Application.Products.Commands.AddPicture;
using U.ProductService.Application.Products.Commands.ChangeCategory;
using U.ProductService.Application.Products.Commands.ChangePrice;
using U.ProductService.Application.Products.Commands.Create;
using U.ProductService.Application.Products.Commands.DeletePicture;
using U.ProductService.Application.Products.Commands.Publish;
using U.ProductService.Application.Products.Commands.UnPublish;
using U.ProductService.Application.Products.Commands.Update;
using U.ProductService.Application.Products.Models;
using U.ProductService.Application.Products.Queries.GetCount;
using U.ProductService.Application.Products.Queries.GetList;
using U.ProductService.Application.Products.Queries.GetSingle;
using U.ProductService.Application.Products.Queries.GetSingleByAlternativeKey;
using U.ProductService.Application.Products.Queries.GetStatistics;
using U.ProductService.Application.Products.Queries.GetStatisticsByCategory;
using U.ProductService.Application.Products.Queries.GetStatisticsByManufacturers;

namespace U.ProductService.Controllers
{
    /// <summary>
    /// Product controller of product service
    /// </summary>
    [Route("api/product/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Product controller of product service
        /// </summary>
        /// <param name="mediator"></param>
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of query
        /// </summary>
        /// <param name="productsListQuery"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsList([FromQuery] GetProductsListQuery productsListQuery)
        {
            var queryResult = await _mediator.Send(productsListQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Get product by its productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(typeof(PaginatedItems<ProductViewModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProduct([FromRoute] Guid productId)
        {
            var queryResult = await _mediator.Send(new QueryProduct(productId));
            return Ok(queryResult);
        }

        /// <summary>
        /// Get product by alternate key
        /// </summary>
        /// <param name="productQuery"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("{alternativeKey}/alternative")]
        [ProducesResponseType(typeof(ProductViewModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProduct([FromRoute] QueryProductByAlternativeKey productQuery)
        {
            var queryResult = await _mediator.Send(productQuery);
            return Ok(queryResult);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand products)
        {
            var productId = await _mediator.Send(products);
            return CreatedAtAction(nameof(GetProduct), new {productId}, productId);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{productId}/update")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProduct([FromQuery] UpdateProductCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Publish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{productId}/publish")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> PublishProduct([FromRoute] Guid productId)
        {
            await _mediator.Send(new PublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// UnPublish Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{productId}/unpublish")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> UnPublishProduct([FromRoute] Guid productId)
        {
            await _mediator.Send(new UnPublishProductCommand(productId));
            return Ok();
        }

        /// <summary>
        /// Change product price
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{productId}/price")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductPrice([FromQuery] ChangeProductPriceCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Add Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("{productId}/picture")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddPicture([FromQuery] AddProductPictureCommand command)
        {
            var pictureId = await _mediator.Send(command);
            return Ok(pictureId);
        }

        /// <summary>
        /// Delete Product Picture
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpDelete]
        [Route("{productId}/picture/{pictureId}")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePicture([FromRoute] DeleteProductPictureCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get Product statistics
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/creation")]
        [ProducesResponseType(typeof(ProductStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductCreationStatistics([FromQuery] GetProductsStatisticsQuery query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
        /// <summary>
        /// Get Product statistics by manufacturer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/manufacturer")]
        [ProducesResponseType(typeof(ProductByManufacturersStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductStatisticsByManufacturer([FromQuery] GetProductsStatisticsByManufacturers query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
        /// <summary>
        /// Get Product statistics by category
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("statistics/category")]
        [ProducesResponseType(typeof(ProductByCategoryStatisticsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductStatisticsByCategory([FromQuery] GetProductsStatisticsByCategory query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }

        /// <summary>
        /// Change product category
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{productId}/category")]
        [ProducesResponseType(typeof(Guid), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ChangeProductCategory([FromQuery] ChangeProductCategoryCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }


        /// <summary>
        /// Get Product total count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("count")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCount([FromQuery] GetProductsCount query)
        {
            var statistics = await _mediator.Send(query);
            return Ok(statistics);
        }
    }
}