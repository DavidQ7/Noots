import { ElementRef, EventEmitter, Injectable, Renderer2 } from '@angular/core';
import { ApiBrowserTextService } from '../../api-browser-text.service';
import { BreakEnterModel, ContentEditableService } from '../../content-editable.service';
import { MenuSelectionService } from '../../menu-selection.service';
import { BaseText, ContentModel, ContentType, HtmlText } from '../../models/ContentMode';
import { EnterEvent } from '../../models/enterEvent';
import { SelectionService } from '../../selection.service';

@Injectable()
export abstract class HtmlService {

  preFocus = false;

  contentStr = '';

  listeners = [];

  constructor(
    public apiBrowserService: ApiBrowserTextService,
    public selectionService: SelectionService,
    public menuSelectionService: MenuSelectionService,
    private renderer: Renderer2,
    public contEditService: ContentEditableService) {
  }

  abstract onInput(content: ContentModel, contentHtml: ElementRef, addToEndNewText?: EventEmitter<any>);

  pasteCommandHandler(e) {
    this.apiBrowserService.pasteCommandHandler(e);
  }

  eventEventFactory(id: string, breakModel: BreakEnterModel, nextItemType: ContentType): EnterEvent {
    const eventModel: EnterEvent = {
      id,
      breakModel,
      nextItemType
    };
    return eventModel;
  }

  abstract onBlur(e);
  abstract onSelectStart(e);
  abstract enter(e, content: ContentModel<BaseText>, contentHtml: ElementRef, enterEvent: EventEmitter<EnterEvent>);
  abstract backUp(e);
  abstract setFocus($event, contentHtml: ElementRef);
  abstract setFocusToEnd(contentHtml: ElementRef);

  backDown($event, content: ContentModel<BaseText>, contentHtml: ElementRef,
    concatThisWithPrev: EventEmitter<string>, deleteThis: EventEmitter<string>) {
    const selection = this.apiBrowserService.getSelection().toString();
    if (this.contEditService.isStart(this.getNativeElement(contentHtml)) && !this.isContentEmpty(contentHtml) && selection === '') {
      $event.preventDefault();
      concatThisWithPrev.emit(content.contentId);
    }

    if (this.isContentEmpty(contentHtml)) {
      $event.preventDefault();
      deleteThis.emit(content.contentId);
    }
  }

  isContentEmpty(contentHtml: ElementRef) {
    return this.getNativeElement(contentHtml)?.textContent.length === 0;
  }

  getNativeElement(contentHtml: ElementRef) {
    return contentHtml?.nativeElement;
  }

  setHandlers(content: ContentModel<HtmlText>, contentHtml: ElementRef, enterEvent: EventEmitter<EnterEvent>,
    concatThisWithPrev: EventEmitter<string>, deleteThis: EventEmitter<string>, addToEndNewText?: EventEmitter<any>) {
    const input = this.renderer.listen(contentHtml.nativeElement, 'input',
      (e) => { this.onInput(content, contentHtml, addToEndNewText); });
    const blur = this.renderer.listen(contentHtml.nativeElement, 'blur', (e) => { this.onBlur(e); });
    const paste = this.renderer.listen(contentHtml.nativeElement, 'paste', (e) => { this.pasteCommandHandler(e); });
    const selectStart = this.renderer.listen(contentHtml.nativeElement, 'selectstart', (e) => { this.onSelectStart(e); });
    const keydownEnter = this.renderer.listen(contentHtml.nativeElement, 'keydown.enter',
      (e) => { this.enter(e, content, contentHtml, enterEvent); });
    const keydownBackspace = this.renderer.listen(contentHtml.nativeElement, 'keydown.backspace',
      (e) => { this.backDown(e, content, contentHtml, concatThisWithPrev, deleteThis); });
    const keyupBackspace = this.renderer.listen(contentHtml.nativeElement, 'keyup.backspace', (e) => { this.backUp(e); });
    this.listeners.push(input, blur, paste, selectStart, keydownBackspace, keydownEnter, keyupBackspace);
  }

  destroysListeners() {
    for (const destroyFunc of this.listeners) {
      destroyFunc();
    }
  }

  isActive(contentHtml: ElementRef) {
    return (this.isContentEmpty(contentHtml) && document.activeElement === this.getNativeElement(contentHtml))
      || (this.preFocus && this.isContentEmpty(contentHtml));
  }

  mouseEnter($event, contentHtml: ElementRef) {
    this.preFocus = !this.selectionService.ismousedown;
  }

  mouseOut($event, contentHtml: ElementRef) {
    this.preFocus = false;
  }

}