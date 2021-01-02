import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiBrowserTextService {

  constructor() { }

  pasteCommandHandler(e) {
    e.preventDefault();
    const text = (e.originalEvent || e).clipboardData.getData('text/plain');
    document.execCommand('insertHTML', false, text);
  }

  copyInputLink(input: HTMLInputElement) {
    input.select();
    document.execCommand('copy');
    input.setSelectionRange(0, 0);
  }
}
