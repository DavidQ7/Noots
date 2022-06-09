import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ApiBrowserTextService } from 'src/app/content/notes/api-browser-text.service';
import { BaseText, NoteTextTypeENUM } from 'src/app/content/notes/models/editor-models/base-text';
import { ThemeENUM } from 'src/app/shared/enums/theme.enum';
import { ContentTypeENUM } from '../../../../models/editor-models/content-types.enum';
import { ClickableContentService } from '../../../content-editor-services/clickable-content.service';
import { SelectionService } from '../../../content-editor-services/selection.service';
import { EnterEvent } from '../../../models/enter-event.model';
import { ParentInteraction } from '../../../models/parent-interaction.interface';
import { TransformContent } from '../../../models/transform-content.model';
import { BaseTextElementComponent } from '../html-base.component';
import { NumberListService } from '../html-business-logic/numberList.service';

@Component({
  selector: 'app-html-number-list',
  templateUrl: './html-number-list.component.html',
  styleUrls: ['./html-number-list.component.scss'],
  providers: [NumberListService],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HtmlNumberListComponent
  extends BaseTextElementComponent
  implements OnInit, OnDestroy, AfterViewInit, ParentInteraction, OnChanges
{
  @Output()
  transformTo = new EventEmitter<TransformContent>();

  @Output()
  enterEvent = new EventEmitter<EnterEvent>();

  @Output()
  deleteThis = new EventEmitter<string>();

  @Output()
  concatThisWithPrev = new EventEmitter<string>();

  @Output()
  // eslint-disable-next-line @angular-eslint/no-output-on-prefix
  onFocus = new EventEmitter<HtmlNumberListComponent>();

  @Input()
  prevContent: BaseText;

  @Input()
  prevType: ContentTypeENUM;

  @Input()
  theme: ThemeENUM;

  themeE = ThemeENUM;

  constructor(
    public numberService: NumberListService,
    private host: ElementRef,
    cdr: ChangeDetectorRef,
    apiBrowserTextService: ApiBrowserTextService,
    selectionService: SelectionService,
    clickableService: ClickableContentService,
  ) {
    super(cdr, apiBrowserTextService, selectionService, clickableService);
  }

  getHost() {
    return this.host;
  }

  ngOnChanges(): void {
    this.setNumber();
  }

  ngAfterViewInit(): void {
    this.numberService.setHandlers(
      this.content,
      this.contentHtml,
      this.enterEvent,
      this.concatThisWithPrev,
      this.deleteThis,
    );
  }

  ngOnDestroy(): void {
    this.numberService.destroysListeners();
    this.destroy.next();
    this.destroy.complete();
  }

  ngOnInit(): void {
    this.initBaseHTML();
    this.numberService.transformTo = this.transformTo;
  }

  setNumber() {
    if (this.prevContent && this.prevContent.noteTextTypeId === NoteTextTypeENUM.Numberlist) {
      this.content.listNumber = this.prevContent.listNumber + 1;
    } else {
      this.content.listNumber = 1;
    }
  }

  isFocusToNext = () => true;

  // eslint-disable-next-line class-methods-use-this
  backspaceUp() {}

  // eslint-disable-next-line class-methods-use-this
  backspaceDown() {}

  // eslint-disable-next-line class-methods-use-this
  deleteDown() {}
}
