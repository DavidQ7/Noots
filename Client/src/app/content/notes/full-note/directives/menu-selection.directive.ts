import { Directive, Input, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { ApiBrowserTextService } from '../../api-browser-text.service';
import { ParentInteraction, ParentInteractionHTML } from '../models/parent-interaction.interface';
import { SelectionService } from '../content-editor-services/selection.service';

@Directive({
  selector: '[appMenuSelection]',
})
export class MenuSelectionDirective implements OnDestroy, OnInit {
  @Input() appMenuSelection: ParentInteractionHTML[];

  @Input() isReadonly: boolean;

  @Input() scrollElement: HTMLElement;

  listeners = [];

  constructor(
    private renderer: Renderer2,
    public apiBrowserService: ApiBrowserTextService,
    public selectionService: SelectionService,
  ) {}

  ngOnInit(): void {
    if (!this.isReadonly) {
      const mouseupListener = this.renderer.listen(document, 'selectionchange', () =>
        this.onSelectionchange(),
      );
      this.listeners.push(mouseupListener);
    }
  }

  onSelectionchange(clearIfEmpty: boolean = false) {
    const selection = this.apiBrowserService.getSelection();
    const selectionEmpty = selection.toString() === '';
    if (!selectionEmpty) {
      const range = selection.getRangeAt(0);
      const coords = range.getBoundingClientRect();
      const currentItem = this.getCurrentItem();
      if (currentItem) {
        this.selectionService.initSingle(currentItem.getContentId(), this.scrollElement, coords);
      }
    }
    if (clearIfEmpty && selectionEmpty) {
      this.selectionService.resetSelectionItems();
    }
  }

  getCurrentItem(): ParentInteraction {
    for (const item of this.appMenuSelection) {
      if (item.getEditableNative() === document.activeElement) {
        return item;
      }
    }
    return null;
  }

  ngOnDestroy(): void {
    for (const destroyFunc of this.listeners) {
      destroyFunc();
    }
  }
}
