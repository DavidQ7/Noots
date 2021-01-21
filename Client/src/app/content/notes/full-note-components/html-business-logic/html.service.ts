import { ElementRef, Injectable, Renderer2 } from '@angular/core';
import { ApiBrowserTextService } from '../../api-browser-text.service';
import { ContentEditableService } from '../../content-editable.service';
import { MenuSelectionService } from '../../menu-selection.service';
import { ContentModel, HtmlText } from '../../models/ContentMode';
import { SelectionService } from '../../selection.service';

@Injectable()
export abstract class HtmlService {

    contentStr = '';

    listeners = [];

    constructor(
        public apiBrowserService: ApiBrowserTextService,
        public selectionService: SelectionService,
        public menuSelectionService: MenuSelectionService,
        private renderer: Renderer2,
        public contEditService: ContentEditableService) {
    }

    onInput(content: ContentModel<HtmlText>, contentHtml: ElementRef)
    {
        content.data.content = contentHtml.nativeElement.innerText;
    }

    pasteCommandHandler(e){
      this.apiBrowserService.pasteCommandHandler(e);
    }

    abstract onBlur(e);
    abstract onSelectStart(e);
    abstract enter(e);
    abstract backDown(e);
    abstract backUp(e);
    abstract getTextChild(contentHtml: ElementRef);

    mouseUp($event: MouseEvent) {
      const selection = this.apiBrowserService.getSelection();
      if (selection.toString() !== '') {
        const coords = selection.getRangeAt(0).getBoundingClientRect();
        this.menuSelectionService.menuActive = true;
        this.menuSelectionService.left = ((coords.left + coords.right) / 2) - this.selectionService.sidebarWidth;
        this.menuSelectionService.top = coords.top - this.selectionService.menuHeight - 45;
      } else {
        this.menuSelectionService.menuActive = false;
      }
    }

    isContentEmpty(contentHtml: ElementRef)
    {
      return this.getTextChild(contentHtml).textContent.length === 0;
    }

    setHandlers(content: ContentModel<HtmlText>, contentHtml: ElementRef)
    {
        const input = this.renderer.listen(contentHtml.nativeElement, 'input', (e) => { this.onInput(content, contentHtml); });
        const blur = this.renderer.listen(contentHtml.nativeElement, 'blur', (e) => { this.onBlur(e); });
        const paste = this.renderer.listen(contentHtml.nativeElement, 'paste', (e) => { this.pasteCommandHandler(e); });
        const mouseUp = this.renderer.listen(contentHtml.nativeElement, 'mouseup', (e) => { this.mouseUp(e); });
        const selectStart = this.renderer.listen(contentHtml.nativeElement, 'selectstart', (e) => { this.onSelectStart(e); });
        const keydownEnter = this.renderer.listen(contentHtml.nativeElement, 'keydown.enter', (e) => { this.enter(e); });
        const keydownBackspace = this.renderer.listen(contentHtml.nativeElement, 'keydown.backspace', (e) => { this.backDown(e); });
        const keyupBackspace = this.renderer.listen(contentHtml.nativeElement, 'keyup.backspace', (e) => { this.backUp(e); });
        this.listeners.push(input, blur, paste, mouseUp, selectStart, keydownBackspace, keydownEnter, keyupBackspace);
    }

    destroysListeners()
    {
        for (const func of this.listeners)
        {
          func();
        }
    }
}
