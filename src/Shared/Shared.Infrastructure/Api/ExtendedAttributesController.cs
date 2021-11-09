using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Api.Contracts;
using Shared.Infrastructure.Api.Filters;

namespace Shared.Infrastructure.Api
{
    [ApiController]
    public abstract class ExtendedAttributesController<TEntityId, TEntity> : CommonBaseController
        where TEntity : class, IEntity<TEntityId>
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync([FromQuery] PaginatedExtendedAttributeFilter<TEntityId, TEntity> filter)
        {
            // var extendedAttributes = await Mediator.Send(new GetExtendedAttributesQuery<TEntityId, TEntity>(filter));
            // return Ok(extendedAttributes);
            await Task.CompletedTask;
            return Ok("extendedAttributes");
        }
    }
}