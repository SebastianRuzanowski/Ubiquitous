using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using U.SmartStoreAdapter.Application.Categories;

namespace U.SmartStoreAdapter.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    [Route("api/smartstore/categories")]
    [ExcludeFromCodeCoverage]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Store Categories
        /// </summary>
        /// <param name="mediator"></param>
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("store")]
        [ProducesResponseType(typeof(CategoryViewModel), 201)]
        public async Task<IActionResult> StoreCategory([FromBody]StoreCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(StoreCategory), result);
        }

        /// <summary>
        /// Get Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<CategoryViewModel>),200)]
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}