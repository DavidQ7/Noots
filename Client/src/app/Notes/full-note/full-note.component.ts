import { Component, OnInit, ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { Subscription, Subject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Label } from 'src/app/Models/Labels/Label';
import { NotesService } from 'src/app/Services/notes.service';
import { UpdateTitle } from 'src/app/Models/Notes/UpdateTitle';
import { FullNote } from 'src/app/Models/Notes/FullNote';
import { PartsService } from 'src/app/Services/parts.service';
import { NewUnknown } from 'src/app/Models/PartUnknown/NewUnknow';
import { takeUntil } from 'rxjs/operators';
import { DeleteUnknown } from 'src/app/Models/PartUnknown/DeleteUnknown';
import { Unknown } from 'src/app/Models/Parts/Unknown';

@Component({
  selector: 'app-full-note',
  templateUrl: './full-note.component.html',
  styleUrls: ['./full-note.component.sass']
})
export class FullNoteComponent implements OnInit {



  // Content
  private title: string;
  private titleTimer;
  unsubscribe = new Subject();
  private id: string;
  public note: FullNote;
  private subscription: Subscription;
  public youtube = false;
  public labels: Label[] = [
    { id: '24', name: 'work', color: '#FFCDCD'},
    { id: '24', name: 'instanse', color: '#FFCDCD'},
    { id: '24', name: 'homework', color: '#FFCDCD'},
    { id: '24', name: 'Database', color: '#FFCDCD'},
    { id: '24', name: 'LoremIpsums124124124', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
    { id: '24', name: '23', color: '#FFCDCD'},
  ];
  constructor(private activateRoute: ActivatedRoute, private noteService: NotesService, private partsService: PartsService) {
    this.subscription = activateRoute.params.subscribe(params => this.id = params.id);
   }

  ngOnInit() {
    this.noteService.getById(this.id)
    .pipe(takeUntil(this.unsubscribe))
    .subscribe(x => {this.note = x; }, error => console.log(error));
  }

  youTubeMenu() {
    this.youtube = !this.youtube;
  }

  changeTitle(event) {
    this.title = event;
    const newTitle: UpdateTitle = {
      id: this.id,
      title: this.title
    };
    clearTimeout(this.titleTimer);
    this.titleTimer = setTimeout(() => this.noteService.updateTitle(newTitle)
    .pipe(takeUntil(this.unsubscribe))
    .subscribe(x => x ), 300);
  }



  newLine(i: number) {
    const part: NewUnknown = {
      noteId: this.note.id,
      index: i
    };
    const unknown: Unknown = {
      id: '',
      type: 'unknown'
    };
    document.getElementById(`${i}`).innerHTML = '';
    this.note.parts.splice(++i, 0, unknown);
    setTimeout(() => document.getElementById(`${i}`).focus(), 50);
    this.partsService.newUnknown(part)
    .pipe(takeUntil(this.unsubscribe))
    .subscribe(x => {
      unknown.id = x;
    }, error => console.log(error));
  }
  newLineUnknown(i: number) {
    const part: NewUnknown = {
      noteId: this.note.id,
      index: i
    };
    const unknown: Unknown = {
      id: '',
      type: 'unknown'
    };
    document.getElementById(`${i}`).innerHTML = '';
    this.note.parts.splice(++i, 0, unknown);
    setTimeout(() => document.getElementById(`${i - 1}`).focus(), 50);
    this.partsService.newUnknown(part)
    .pipe(takeUntil(this.unsubscribe))
    .subscribe(x => {
      unknown.id = x;
    }, error => console.log(error));
  }
  deleteLine(id: string, index: number) {
    const part: DeleteUnknown = {
      noteId: this.note.id,
      partId: id
    };
    this.partsService.deleteUnknown(part)
    .pipe(takeUntil(this.unsubscribe))
    .subscribe(x => {
      this.note.parts = this.note.parts.filter(z => z.id !== id);
      if (index === 0) {
      return;
      }
      document.getElementById(`${--index}`).focus();
    }, error => console.log(error));
  }
  up(index: number) {
    const element = document.getElementById(`${index - 1}`);
    if (element !== null) {
    element.focus();
    }
  }
  down(index: number) {
    const element = document.getElementById(`${index + 1}`);
    if (element !== null) {
    element.focus();
    }
  }
}
