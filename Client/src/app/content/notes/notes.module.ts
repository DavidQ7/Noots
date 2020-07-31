import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotesComponent } from './notes/notes.component';
import { NoteRouting } from './notes-routing';
import { SharedModule } from 'src/app/shared/shared.module';
import { FullNoteComponent } from './full-note/full-note.component';
import { NoteComponent } from './note/note.component';
import { PrivatesComponent } from './privates/privates.component';
import { SharedComponent } from './shared/shared.component';
import { DeletedComponent } from './deleted/deleted.component';
import { ArchiveComponent } from './archive/archive.component';


@NgModule({
  declarations: [NotesComponent, FullNoteComponent, NoteComponent, PrivatesComponent, SharedComponent, DeletedComponent, ArchiveComponent],
  imports: [
    CommonModule,
    NoteRouting,
    SharedModule
  ]
})
export class NotesModule { }
