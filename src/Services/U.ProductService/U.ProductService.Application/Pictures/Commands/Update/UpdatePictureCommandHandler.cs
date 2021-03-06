﻿using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using U.ProductService.Application.Common.Exceptions;
using U.ProductService.Application.Pictures.Models;
using U.ProductService.Domain.Common;
using U.ProductService.Persistance.Repositories.Picture;

namespace U.ProductService.Application.Pictures.Commands.Update
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class UpdatePictureCommandHandler : IRequestHandler<UpdatePictureCommand, PictureViewModel>
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IDomainEventsService _domainEventsService;

        public UpdatePictureCommandHandler(IPictureRepository productRepository,
            IMapper mapper,
            IMediator mediator,
            IDomainEventsService domainEventsService)
        {
            _pictureRepository = productRepository;
            _mapper = mapper;
            _mediator = mediator;
            _domainEventsService = domainEventsService;
        }

        public async Task<PictureViewModel> Handle(UpdatePictureCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdatePictureCommand.Validator();
            await validator.ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

            var picture = await _pictureRepository.GetAsync(command.PictureId);

            if (picture is null)
            {
                throw new PictureNotFoundException($"Picture with id: '{command.PictureId}' has not been found");
            }

            picture.Update(command.FileStorageUploadId,
                command.Filename,
                command.Description,
                command.Url,
                command.MimeTypeId);

            _pictureRepository.Update(picture);

            await _pictureRepository.UnitOfWork.SaveEntitiesAsync(_domainEventsService, _mediator, cancellationToken);

            return _mapper.Map<PictureViewModel>(picture);
        }
    }
}