import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PersonalizationService } from 'src/app/shared/services/personalization.service';
import { FontSizeENUM } from 'src/app/shared/enums/FontSizeEnum';
import { SmallNote } from '../models/smallNote';
import { ContentType } from '../models/ContentMode';

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.scss'],
})
export class NoteComponent {
  @Input() note: SmallNote;

  @Output() highlightNote = new EventEmitter<SmallNote>();

  @Output() clickOnNote = new EventEmitter<SmallNote>();

  fontSize = FontSizeENUM;

  contentType = ContentType;

  constructor(public pService: PersonalizationService) {}

  highlight(note: SmallNote) {
    this.highlightNote.emit(note);
  }

  toNote(note: SmallNote) {
    this.clickOnNote.emit(note);
  }
}
