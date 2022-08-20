﻿using Common;
using Common.DTO;
using Common.DTO.Backgrounds;
using MediatR;
using Noots.Backgrounds.Commands;
using Noots.DatabaseContext.Repositories.Users;
using Noots.Mapper.Mapping;
using Noots.Permissions.Queries;
using Noots.Storage.Commands;

namespace Noots.Backgrounds.Handlers.Commands;

public class NewBackgroundCommandHandler : IRequestHandler<NewBackgroundCommand, OperationResult<BackgroundDTO>>
{
    private readonly UserRepository userRepository;
    private readonly BackgroundRepository backgroundRepository;
    private readonly IMediator _mediator;
    private readonly UserBackgroundMapper userBackgroundMapper;
    
    public NewBackgroundCommandHandler(
        UserRepository userRepository, 
        BackgroundRepository backgroundRepository, 
        IMediator mediator, 
        UserBackgroundMapper userBackgroundMapper)
    {
        this.userRepository = userRepository;
        this.backgroundRepository = backgroundRepository;
        _mediator = mediator;
        this.userBackgroundMapper = userBackgroundMapper;
    }
    
    public async Task<OperationResult<BackgroundDTO>> Handle(NewBackgroundCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == request.UserId);
        var uploadPermission = await _mediator.Send(new GetPermissionUploadFileQuery(request.File.Length, user.Id));
        if(uploadPermission == PermissionUploadFileEnum.NoCanUpload)
        {
            return new OperationResult<BackgroundDTO>().SetNoEnougnMemory();
        }

        var filebytes = await request.File.GetFilesBytesAsync();
        var appFile = await _mediator.Send(new SaveBackgroundCommand(user.Id, filebytes));

        var item = new Common.DatabaseModels.Models.Users.Background()
        {
            UserId = user.Id,
            File = appFile
        };

        var success = await backgroundRepository.AddBackground(item, appFile);

        if (!success)
        {
            await _mediator.Send(new RemoveFilesCommand(user.Id.ToString(), appFile));
            return null;
        }

        await _mediator.Send(new UpdateBackgroundCommand(request.UserId, item.Id), CancellationToken.None);
        var ent = userBackgroundMapper.MapToBackgroundDTO(item);
        return new OperationResult<BackgroundDTO>(success: true, ent);
    }
}