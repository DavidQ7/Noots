import { ElementRef, EventEmitter, Injectable } from '@angular/core';
import { BaseText, NoteTextTypeENUM } from '../../../../models/content-model.model';
import { EnterEvent } from '../../../models/enter-event.model';
import { TransformContent } from '../../../models/transform-content.model';
import { HtmlService } from './html.service';

@Injectable()
export class CheckListService extends HtmlService {
  transformTo = new EventEmitter<TransformContent>();

  onBlur = (e: any) => {
    // BLUR HANDLER
  };

  pasteCommandHandler = (e: any) => {
    throw new Error('Method not implemented.');
  };

  onSelectStart = (e: any) => {
    // SELECTIION
  };

  enter(
    $event: any,
    content: BaseText,
    contentHtml: ElementRef,
    enterEvent: EventEmitter<EnterEvent>,
  ) {
    $event.preventDefault();
    if (this.isContentEmpty(contentHtml)) {
      this.transformTo.emit({
        id: content.id,
        textType: NoteTextTypeENUM.Default,
        setFocusToEnd: true,
      });
    } else {
      const breakModel = this.contEditService.pressEnterHandler(this.getNativeElement(contentHtml));
      content.contentSG = contentHtml.nativeElement.textContent;
      const event = super.eventEventFactory(breakModel, NoteTextTypeENUM.Checklist, content.id);
      enterEvent.emit(event);
    }
  }

  checkForDelete(
    $event,
    content: BaseText,
    contentHtml: ElementRef,
    concatThisWithPrev: EventEmitter<string>,
    deleteThis: EventEmitter<string>,
  ) {
    super.checkForDelete($event, content, contentHtml, concatThisWithPrev, deleteThis);
  }

  backUp = (e: any) => {};
}