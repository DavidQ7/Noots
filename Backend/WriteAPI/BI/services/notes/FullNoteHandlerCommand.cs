﻿using Common.Naming;
using Domain.Commands.noteInner;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WriteContext.Repositories;

namespace BI.services.notes
{
    public class FullNoteHandlerCommand :
        IRequestHandler<UpdateTitleNoteCommand, Unit>,
        IRequestHandler<UploadImageToNoteCommand, Unit>
    {
        private readonly UserRepository userRepository;
        private readonly NoteRepository noteRepository;
        public FullNoteHandlerCommand(UserRepository userRepository, NoteRepository noteRepository)
        {
            this.userRepository = userRepository;
            this.noteRepository = noteRepository;
        }

        public async Task<Unit> Handle(UpdateTitleNoteCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByEmail(request.Email);
            if (user != null)
            {
                var note = await this.noteRepository.GetForUpdatingTitle(request.Id);
                switch (note.NoteType.Name)
                {
                    case ModelsNaming.SharedNote:
                        {
                            switch (note.RefType.Name)
                            {
                                case ModelsNaming.Editor:
                                    {
                                        throw new Exception("No implimented");
                                    }
                                case ModelsNaming.Viewer:
                                    {
                                        throw new Exception("No implimented");
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            if (note.UserId == user.Id)
                            {
                                note.Title = request.Title;
                                await noteRepository.UpdateNote(note);
                            }
                            else
                            {
                                var noteUser = note.UsersOnPrivateNotes.FirstOrDefault(x => x.UserId == user.Id);
                                if (noteUser != null && noteUser.AccessType.Name == ModelsNaming.Editor)
                                {
                                    note.Title = request.Title;
                                    await noteRepository.UpdateNote(note);
                                }
                                else
                                {
                                    throw new Exception("No access rights");
                                }
                            }
                            break;
                        }
                }
            }
            return Unit.Value;
        }

        public async Task<Unit> Handle(UploadImageToNoteCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("TODO");
            return Unit.Value;
        }
    }
}
