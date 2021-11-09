using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Shared.Infrastructure.Caching;
using Shared.Infrastructure.Entities;
using Shared.Infrastructure.Exceptions;
using Shared.Infrastructure.Utilities;
using Shared.Infrastructure.Wrapper;

namespace Shared.Infrastructure.CQRS.Commands
{
    public class ExtendedAttributeCommandHandler
    {
        
    }

    public class ExtendedAttributeCommandHandler<TEntityId, TEntity, TExtendedAttribute> :
        IRequestHandler<RemoveExtendedAttributeCommand<TEntityId, TEntity>, Result<long>>,
        IRequestHandler<AddExtendedAttributeCommand<TEntityId, TEntity>, Result<long>>,
        IRequestHandler<UpdateExtendedAttributeCommand<TEntityId, TEntity>, Result<long>>
        where TEntity : class, IEntity<TEntityId>
        where TExtendedAttribute : ExtendedAttribute<TEntityId, TEntity>
    {
        private readonly IDistributedCache _cache;
        private readonly IExtendedAttributeDbContext<TEntityId, TEntity, TExtendedAttribute> _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ExtendedAttributeCommandHandler> _localizer;
        
        
        public ExtendedAttributeCommandHandler(
            IExtendedAttributeDbContext<TEntityId, TEntity, TExtendedAttribute> context,
            IMapper mapper,
            IStringLocalizer<ExtendedAttributeCommandHandler> localizer,
            IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
            _cache = cache;
        }
        
        public async Task<Result<long>> Handle(AddExtendedAttributeCommand<TEntityId, TEntity> request, CancellationToken cancellationToken)
        {
            var entity = await _context.Entities.AsNoTracking().FirstOrDefaultAsync(e => e.Id != null && e.Id.Equals(request.EntityId), cancellationToken);
            if (entity == null)
            {
                throw new CustomException(string.Format(_localizer["{0} Not Found"], typeof(TEntity).GetGenericTypeName()), statusCode: HttpStatusCode.NotFound);
            }
            
            bool isKeyUsed = await _context.ExtendedAttributes.AsNoTracking()
                .AnyAsync(ea => ea.EntityId != null && ea.EntityId.Equals(request.EntityId) && ea.Key.Equals(request.Key), cancellationToken);
            if (isKeyUsed)
            {
                throw new CustomException(string.Format(_localizer["This {0} Key is Already Used For This Entity"], typeof(TEntity).GetGenericTypeName()), statusCode: HttpStatusCode.NotFound);
            }
            
            var extendedAttribute = _mapper.Map<TExtendedAttribute>(request);
            // extendedAttribute.AddDomainEvent(new ExtendedAttributeAddedEvent<TEntityId, TEntity>(extendedAttribute));
            await _context.ExtendedAttributes.AddAsync(extendedAttribute, cancellationToken);
            // await _context.SaveChangesAsync(cancellationToken);
            return await Result<long>.SuccessAsync(extendedAttribute.Id, string.Format(_localizer["{0} Extended Attribute Saved"], typeof(TEntity).GetGenericTypeName()));
        }

        public async Task<Result<long>> Handle(RemoveExtendedAttributeCommand<TEntityId, TEntity> request, CancellationToken cancellationToken)
        {
            var extendedAttribute = await _context.ExtendedAttributes.FirstOrDefaultAsync(ea => ea.Id == request.Id, cancellationToken);
            if (extendedAttribute == null)
            {
                throw new CustomException(string.Format(_localizer["{0} Extended Attribute Not Found"], typeof(TEntity).GetGenericTypeName()), statusCode: HttpStatusCode.NotFound);
            }

            _context.ExtendedAttributes.Remove(extendedAttribute);
            // extendedAttribute.AddDomainEvent(new ExtendedAttributeRemovedEvent<TEntity>(request.Id));
            // await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<long, ExtendedAttribute<TEntityId, TEntity>>(request.Id), cancellationToken);
            return await Result<long>.SuccessAsync(extendedAttribute.Id, string.Format(_localizer["{0} Extended Attribute Deleted"], typeof(TEntity).GetGenericTypeName()));
        }

        public async Task<Result<long>> Handle(UpdateExtendedAttributeCommand<TEntityId, TEntity> request, CancellationToken cancellationToken)
        {
            var extendedAttribute = await _context.ExtendedAttributes.Where(ea => ea.Id.Equals(request.Id)).FirstOrDefaultAsync(cancellationToken);
            if (extendedAttribute == null)
            {
                throw new CustomException(string.Format(_localizer["{0} Extended Attribute Not Found!"], typeof(TEntity).GetGenericTypeName()), statusCode: HttpStatusCode.NotFound);
            }

            if (extendedAttribute.EntityId != null && !extendedAttribute.EntityId.Equals(request.EntityId))
            {
                throw new CustomException(string.Format(_localizer["{0} Not Found"], typeof(TEntity).GetGenericTypeName()), statusCode: HttpStatusCode.NotFound);
            }

            bool isKeyUsed = await _context.ExtendedAttributes.AsNoTracking()
                .AnyAsync(ea => ea.EntityId != null && ea.Id != extendedAttribute.Id && ea.EntityId.Equals(request.EntityId) && ea.Key.Equals(request.Key), cancellationToken);
            if (isKeyUsed)
            {
                throw new CustomException(string.Format(_localizer["This {0} Key Is Already Used For This Entity"], typeof(TEntity).GetGenericTypeName()), statusCode: HttpStatusCode.NotFound);
            }

            extendedAttribute = _mapper.Map(request, extendedAttribute);
            // extendedAttribute.AddDomainEvent(new ExtendedAttributeUpdatedEvent<TEntityId, TEntity>(extendedAttribute));
            _context.ExtendedAttributes.Update(extendedAttribute);
            // await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync(CacheKeys.Common.GetEntityByIdCacheKey<long, ExtendedAttribute<TEntityId, TEntity>>(request.Id), cancellationToken);
            return await Result<long>.SuccessAsync(extendedAttribute.Id, string.Format(_localizer["{0} Extended Attribute Updated"], typeof(TEntity).GetGenericTypeName()));

        }
    }
}