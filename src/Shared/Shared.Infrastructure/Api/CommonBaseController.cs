using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Api
{
    [ApiController]
    [Route(BasePath + "/[controller]")]
    public abstract class CommonBaseController : ControllerBase
    {
        protected internal const string BasePath = "api";

        private IMediator? _mediatorInstance;
        
        protected IMediator? Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        
        private IMapper? _mapperInstance;
        
        protected IMapper? Mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}