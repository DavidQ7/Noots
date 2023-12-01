import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { PersonalizationService } from 'src/app/shared/services/personalization.service';
import { NoteTypeENUM } from 'src/app/shared/enums/note-types.enum';
import { Select, Store } from '@ngxs/store';
import { EntitiesSizeENUM } from 'src/app/shared/enums/font-size.enum';
import { Observable } from 'rxjs';
import { NotesService } from '../notes.service';
import { NoteStore } from '../state/notes-state';
import { UnSelectAllNote } from '../state/notes-actions';
import { UserStore } from 'src/app/core/stateUser/user-state';
import { ShortUser } from 'src/app/core/models/user/short-user.model';
import { BaseNotesComponent } from '../base-notes-component';

@Component({
  selector: 'app-privates',
  templateUrl: './privates.component.html',
  styleUrls: ['./privates.component.scss'],
  providers: [NotesService],
})
export class PrivatesComponent
  extends BaseNotesComponent
  implements OnInit, OnDestroy, AfterViewInit
{
  @Select(NoteStore.privateCount)
  privateCount$: Observable<number>;

  @Select(UserStore.getUserFontSize)
  public fontSize$?: Observable<EntitiesSizeENUM>;
  
  @Select(UserStore.getUser)
  public user$: Observable<ShortUser>;

  fontSize = EntitiesSizeENUM;

  loaded = false;

  constructor(
    public pService: PersonalizationService,
    private store: Store,
    noteService: NotesService,
  ) {
    super(noteService);
  }

  ngAfterViewInit(): void {
    this.noteService.murriInitialise();
  }

  async ngOnInit() {
    this.pService.setSpinnerState(true);
    await this.loadContent();
  }

  async loadContent(typeENUM = NoteTypeENUM.Private) {
    await this.noteService.loadNotes(typeENUM);

    await this.noteService.initializeEntities();

    await this.pService.waitPreloading();
    this.pService.setSpinnerState(false);
    this.loaded = true;
  }

  ngOnDestroy(): void {
    this.store.dispatch(new UnSelectAllNote());
  }
}
