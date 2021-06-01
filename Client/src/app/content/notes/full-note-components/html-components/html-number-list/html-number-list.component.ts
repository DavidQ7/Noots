import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { updateNoteContentDelay } from 'src/app/core/defaults/bounceDelay';
import { BaseText, ContentType } from '../../../models/ContentModel';
import { EditTextEventModel } from '../../../models/EditTextEventModel';
import { EnterEvent } from '../../../models/enterEvent';
import { ParentInteraction } from '../../../models/parent-interaction.interface';
import { TransformContent } from '../../../models/transform-content';
import { NumberListService } from '../../html-business-logic/numberList.service';

@Component({
  selector: 'app-html-number-list',
  templateUrl: './html-number-list.component.html',
  styleUrls: ['./html-number-list.component.scss'],
  providers: [NumberListService],
})
export class HtmlNumberListComponent
  implements OnInit, OnDestroy, AfterViewInit, ParentInteraction, OnChanges {
  @Output()
  updateText = new EventEmitter<EditTextEventModel>();

  @Output()
  transformTo = new EventEmitter<TransformContent>();

  @Output()
  enterEvent = new EventEmitter<EnterEvent>();

  @Output()
  deleteThis = new EventEmitter<string>();

  @Output()
  concatThisWithPrev = new EventEmitter<string>();

  @Input()
  prevContent: BaseText;

  @Input()
  prevType: ContentType;

  @Input()
  content: BaseText;

  @ViewChild('contentHtml') contentHtml: ElementRef;

  textChanged: Subject<string> = new Subject<string>();

  destroy = new Subject<void>();

  constructor(public numberService: NumberListService) {}

  getContent() {
    return this.content;
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
    this.numberService.transformTo = this.transformTo;

    this.textChanged
      .pipe(takeUntil(this.destroy), debounceTime(updateNoteContentDelay))
      .subscribe((str) => {
        this.content.content = str;
        this.updateText.emit({ content: str, contentId: this.content.id });
      });
  }

  setNumber() {
    if (this.prevContent && this.prevContent.type === ContentType.NUMBERLIST) {
      this.content.number = this.prevContent.number + 1;
    } else {
      this.content.number = 1;
    }
  }

  setFocus($event?) {
    this.numberService.setFocus($event, this.contentHtml);
  }

  setFocusToEnd() {
    this.numberService.setFocusToEnd(this.contentHtml);
  }

  updateHTML(content: string) {
    this.content.content = content;
    this.contentHtml.nativeElement.innerHTML = content;
  }

  getNative() {
    return this.contentHtml?.nativeElement;
  }

  mouseEnter($event) {
    this.numberService.mouseEnter($event, this.contentHtml);
  }

  mouseOut($event) {
    this.numberService.mouseOut($event, this.contentHtml);
  }

  get isActive() {
    return this.numberService.isActive(this.contentHtml);
  }

  onInput($event) {
    this.textChanged.next($event.target.innerText);
  }
}
