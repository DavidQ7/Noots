import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BaseText, TextBlock } from '../../../models/editor-models/base-text';
import { DeltaConverter } from '../../content-editor/converter/delta-converter';
import { BaseHtmlComponent } from '../base-html-components';
import { InputHtmlEvent } from './models/input-html-event';

@Component({
  template: '',
})
// eslint-disable-next-line @angular-eslint/component-class-suffix
export class HtmlBaseService extends BaseHtmlComponent {
  @Input()
  content: BaseText;

  @Output()
  inputHtmlEvent = new EventEmitter<InputHtmlEvent>();

  @ViewChild('contentHtml') contentHtml: ElementRef;

  textChanged: Subject<string> = new Subject();

  destroy = new Subject<void>();

  viewHtml: string;

  constructor(cdr: ChangeDetectorRef) {
    super(cdr);

    this.textChanged.pipe(takeUntil(this.destroy)).subscribe(() => {
      if (!this.contentHtml) return;
      this.inputHtmlEvent.emit({
        content: this.content,
        html: this.contentHtml.nativeElement.innerHTML,
      });
    });
  }

  onInput() {
    this.syncHtmlWithLayout();
  }

  syncHtmlWithLayout() {
    this.textChanged.next();
  }

  initBaseHTML(): void {
    console.log('init');
    const delta = DeltaConverter.convertToDelta(this.content.contents);
    this.viewHtml = DeltaConverter.convertDeltaToHtml(delta);
    this.syncHtmlWithLayout();
  }

  updateHTML(contents: TextBlock[]) {
    const delta = DeltaConverter.convertToDelta(contents);
    const html = DeltaConverter.convertDeltaToHtml(delta);
    this.updateNativeHTML(html);
    this.syncHtmlWithLayout();
  }

  syncContentWithLayout() {
    const sel = document.getSelection();
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const saved = [sel.focusNode, sel.focusOffset];
    // this.updateHTML(content.contents);
    console.log('focus');
    setTimeout(() => this.contentHtml.nativeElement.focus(), 2000);
  }

  getContent(): BaseText {
    return this.content;
  }

  getContentId(): string {
    return this.content.id;
  }

  getEditableNative<T>() {
    return this.contentHtml?.nativeElement as T;
  }

  getTextBlocks(): TextBlock[] {
    const html = this.getEditableNative<HTMLElement>().innerHTML;
    const delta = DeltaConverter.convertHTMLToDelta(html);
    return DeltaConverter.convertToTextBlocks(delta);
  }

  private updateNativeHTML(html: string): void {
    this.contentHtml.nativeElement.innerHTML = html;
  }
}
