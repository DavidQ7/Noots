import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { SmallNote } from '../models/smallNote';
import { PersonalizationService } from 'src/app/shared/services/personalization.service';
import { Store } from '@ngxs/store';
import { UnSelectAllNote, LoadDeletedNotes, PositionNote, LoadAllExceptNotes } from '../state/notes-actions';
import { take, takeUntil } from 'rxjs/operators';
import { Order, OrderEntity } from 'src/app/shared/services/order.service';
import { UpdateColor } from '../state/updateColor';
import { NoteType } from 'src/app/shared/enums/NoteTypes';
import { UserStore } from 'src/app/core/stateUser/user-state';
import {  UpdateRoute } from 'src/app/core/stateApp/app-action';
import { EntityType } from 'src/app/shared/enums/EntityTypes';
import { NoteStore } from '../state/notes-state';
import { FontSize } from 'src/app/shared/enums/FontSize';
import { MurriService } from 'src/app/shared/services/murri.service';
import { NotesService } from '../notes.service';

@Component({
  selector: 'app-deleted',
  templateUrl: './deleted.component.html',
  styleUrls: ['./deleted.component.scss']
})
export class DeletedComponent implements OnInit, OnDestroy {

  fontSize = FontSize;
  destroy = new Subject<void>();

  public notes: SmallNote[];

  constructor(public pService: PersonalizationService,
              private store: Store,
              private murriService: MurriService,
              private noteService: NotesService) { }

  async ngOnInit() {

    await this.store.dispatch(new UpdateRoute(EntityType.NoteDeleted)).toPromise();

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
    await this.store.dispatch(new LoadDeletedNotes()).toPromise();

    this.store.dispatch(new LoadAllExceptNotes(NoteType.Deleted));

    this.store.select(NoteStore.deletedNotes).pipe(take(1))
      .subscribe(x => { this.notes = [...x].map(note => { note = { ...note }; return note; });
                        setTimeout(() => this.murriService.initMurriNote(EntityType.NoteDeleted)); });

    this.store.select(NoteStore.updateColorEvent)
      .pipe(takeUntil(this.destroy))
      .subscribe(x => this.noteService.changeColorHandler(this.notes, x));

    this.store.select(NoteStore.removeFromMurriEvent)
      .pipe(takeUntil(this.destroy))
      .subscribe(x => this.delete(x));
  }


  delete(ids: string[]) {
    if (ids.length > 0) {
      this.notes = this.notes.filter(x => ids.indexOf(x.id) !== -1 ? false : true);
      setTimeout(() => this.pService.grid.refreshItems().layout(), 0);
    }
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.complete();
    this.store.dispatch(new UnSelectAllNote());
  }

}
