import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
import { SmallNote } from '../models/smallNote';
import { PersonalizationService } from 'src/app/shared/services/personalization.service';
import { LoadPrivateNotes, UnSelectAllNote, PositionNote, LoadAllExceptNotes } from '../state/notes-actions';
import { Order, OrderEntity } from 'src/app/shared/services/order.service';
import { take, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { UpdateColor } from '../state/updateColor';
import { NoteType } from 'src/app/shared/enums/NoteTypes';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { UpdateRoute } from 'src/app/core/stateApp/app-action';
import { EntityType } from 'src/app/shared/enums/EntityTypes';
import { NoteStore } from '../state/notes-state';
import { FontSize } from 'src/app/shared/enums/FontSize';
import { MurriService } from 'src/app/shared/services/murri.service';
import { NotesService } from '../notes.service';

@Component({
  selector: 'app-privates',
  templateUrl: './privates.component.html',
  styleUrls: ['./privates.component.scss']
})
export class PrivatesComponent implements OnInit, OnDestroy {

  fontSize = FontSize;
  destroy = new Subject<void>();

  public notes: SmallNote[];

  constructor(public pService: PersonalizationService,
              private store: Store,
              private murriService: MurriService,
              private noteService: NotesService) { }


  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.complete();
    this.store.dispatch(new UnSelectAllNote());
  }

  async ngOnInit() {

    await this.store.dispatch(new UpdateRoute(EntityType.NotePrivate)).toPromise();

    this.store.select(UserStore.getTokenUpdated)
    .pipe(takeUntil(this.destroy))
    .subscribe(async (x: boolean) => {
      if (x) {
        await this.loadContent();
      }
    }
    );

  }

  async loadContent() {
    await this.store.dispatch(new LoadPrivateNotes()).toPromise();

    this.store.dispatch(new LoadAllExceptNotes(NoteType.Private));

    this.store.select(NoteStore.privateNotes).pipe(take(1))
      .subscribe(x => { this.notes = [...x].map(note => { note = { ...note }; return note; });
                        setTimeout(() => this.murriService.initMurriNote(EntityType.NotePrivate)); });

    this.store.select(NoteStore.updateColorEvent)
      .pipe(takeUntil(this.destroy))
      .subscribe(x => this.noteService.changeColorHandler(this.notes, x));

    this.store.select(NoteStore.removeFromMurriEvent)
      .pipe(takeUntil(this.destroy))
      .subscribe(x => this.delete(x));

    this.store.select(NoteStore.notesAddingPrivate)
      .pipe(takeUntil(this.destroy))
      .subscribe(x => this.addToDom(x));
  }


  delete(ids: string[]) {
    if (ids.length > 0) {
      this.notes = this.notes.filter(x => ids.indexOf(x.id) !== -1 ? false : true);
      setTimeout(() => this.pService.grid.refreshItems().layout(), 0);
    }
  }

  addToDom(notes: SmallNote[]) {
    if (notes.length > 0) {
      this.notes = [...notes.map(note => { note = { ...note }; return note; }).reverse() , ...this.notes];
      setTimeout(() => {
        const DOMnodes = document.getElementsByClassName('grid-item');
        for (let i = 0; i < notes.length; i++) {
          const el = DOMnodes[i];
          this.pService.grid.add(el, {index : 0, layout: true});
        }
      }, 0);
    }
  }
}
