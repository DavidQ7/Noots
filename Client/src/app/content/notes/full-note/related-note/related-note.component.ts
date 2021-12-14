import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MurriService } from 'src/app/shared/services/murri.service';
import { ChangeStateRelatedNote } from '../models/change-state-related-note.model';
import { ContentTypeENUM } from '../../models/editor-models/content-types.enum';
import { RelatedNote } from '../../models/related-note.model';
import { NoteTextTypeENUM } from '../../models/editor-models/base-text';
@Component({
  selector: 'app-related-note',
  templateUrl: './related-note.component.html',
  styleUrls: ['./related-note.component.scss'],
  providers: [],
})
export class RelatedNoteComponent {
  @Input() note: RelatedNote;

  @Output() deleteNote = new EventEmitter<string>();

  @Output() changeState = new EventEmitter<ChangeStateRelatedNote>();

  contentType = ContentTypeENUM;

  textType = NoteTextTypeENUM;

  constructor(public murriService: MurriService) {}

  turnUpSmallNote() {
    this.changeState.emit({ isOpened: !this.note.isOpened, relatedNoteId: this.note.id });
    this.note.isOpened = !this.note.isOpened;
    setTimeout(() => this.murriService.grid.refreshItems().layout(), 100);
  }

  deleteSmallNote() {
    this.deleteNote.emit(this.note.id);
  }
}
