import { Directive, ElementRef, EventEmitter, HostListener, Output, Renderer2 } from '@angular/core';
import { SelectionService } from '../selection.service';


@Directive({
  selector: '[appSelection]'
})
export class SelectionDirective {

  @Output()
  selectionEvent = new EventEmitter<DOMRect>();

  @Output()
  selectionStartEvent = new EventEmitter<DOMRect>();


  x;
  y;
  finX;
  finY;
  isFullNote = false;
  startTop: number;

  div: HTMLElement;
  mainContent: Element;


  constructor(private elementRef: ElementRef,
              private renderer: Renderer2,
              private selectionService: SelectionService) {

    setTimeout(() => this.init(), 1000); // TODO CHANGE
  }

  init() {
    this.div = this.renderer.createElement('div');
    this.div.classList.add('full-note-selection');

    this.mainContent = document.getElementsByClassName('main-content')[0];
    this.mainContent.appendChild(this.div);

    this.mainContent.addEventListener('scroll', (e) => this.scrollEvent(e));

    document.addEventListener('mousedown', (e) => this.mouseDown(e)); // TODO make unsubscribe
    document.addEventListener('mouseup', (e) => this.mouseUp(e));
    document.addEventListener('mousemove', (e) => this.mouseMove(e));
  }

  @HostListener('mousedown', ['$event'])
  onClick(event) {
    this.isFullNote = true;
  }

  mouseDown(evt) {
    const rectSize = this.div.getBoundingClientRect();
    if (rectSize.width === 0 || rectSize.height === 0) {
      rectSize.x = 0;
      rectSize.y = 0;
      this.selectionEvent.emit(rectSize);
    }

    this.x = evt.pageX;
    this.y = evt.pageY;
    this.startTop = this.mainContent.scrollTop;
    this.div.style.top = (this.y - this.selectionService.menuHeight + this.startTop) + 'px';
    this.div.style.left = (this.x - this.selectionService.sidebarWidth) + 'px';

    this.selectionService.ismousedown = true;
    this.selectionStartEvent.emit(this.div.getBoundingClientRect());
  }

  mouseUp(evt) {
    this.isFullNote = false;
    this.selectionService.ismousedown = false;
    this.startTop = 0;

    this.div.style.width = 0 + 'px';
    this.div.style.height = 0 + 'px';
  }

  mouseMove(evt) {
    if (this.selectionService.ismousedown && this.isFullNote) {
      this.finX = evt.pageX;
      this.finY = evt.pageY;
      const newValueX = (this.finX - this.x);

      let newValueY = 0;
      if (this.startTop !== this.mainContent.scrollTop) {
        newValueY = (this.finY - this.y + this.mainContent.scrollTop - this.startTop);
      } else {
        newValueY = (this.finY - this.y);
      }

      if (newValueY < 0 && newValueX > 0) {
        this.div.style.top = (evt.pageY - this.selectionService.menuHeight +
          this.startTop - this.subtractionScrollTopAndScrollStart) + 'px';
      } else if (newValueY > 0 && newValueX < 0) {
        this.div.style.left = (evt.pageX - this.selectionService.sidebarWidth) + 'px';
      } else if (newValueY < 0 && newValueX < 0) {
        this.div.style.top = (evt.pageY - this.selectionService.menuHeight
          + this.startTop - this.subtractionScrollTopAndScrollStart) + 'px';
        this.div.style.left = (evt.pageX - this.selectionService.sidebarWidth) + 'px';
      }
      this.div.style.width = Math.abs(newValueX) + 'px';
      this.div.style.height = Math.abs(newValueY) + 'px';

      this.selectionEvent.emit(this.div.getBoundingClientRect());
    }
  }

  get subtractionScrollTopAndScrollStart()
  {
    return Math.abs(this.mainContent.scrollTop - this.startTop);
  }

  scrollEvent(e) {
    if (this.selectionService.ismousedown && this.isFullNote) {


      let newValueY = 0;
      if (this.startTop !== this.mainContent.scrollTop) {
        newValueY = (this.finY - this.y + this.mainContent.scrollTop - this.startTop);
      } else {
        newValueY = (this.finY - this.y);
      }

      if (newValueY > 0) {
        this.div.style.height = newValueY + 'px';
      } else {
        this.div.style.top = (this.finY - this.selectionService.menuHeight + this.startTop -
          this.subtractionScrollTopAndScrollStart) + 'px';
        this.div.style.height = Math.abs(newValueY) + 'px';
      }
    }
  }
}