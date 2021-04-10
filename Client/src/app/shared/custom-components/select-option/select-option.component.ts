import { Highlightable } from '@angular/cdk/a11y';
import { Component, HostBinding, HostListener, Input } from '@angular/core';
import { SelectService } from '../../services/select.service';
import { SelectComponent } from '../select/select.component';

@Component({
  selector: 'app-select-option',
  templateUrl: './select-option.component.html',
  styleUrls: ['./select-option.component.scss'],
})
export class SelectOptionComponent implements Highlightable {
  @Input()
  public value: string;

  @HostBinding('class.active')
  public active = false;

  private select: SelectComponent;

  @HostBinding('class.selected')
  public get selected(): boolean {
    return this.select.selectedOption === this;
  }

  constructor(private selectService: SelectService) {
    this.select = this.selectService.getSelect();
  }

  @HostListener('click', ['$event'])
  public onClick(event: UIEvent) {
    event.preventDefault();
    event.stopPropagation();

    this.select.selectOption(this);
  }

  public setActiveStyles(): void {
    this.active = true;
  }

  public setInactiveStyles(): void {
    this.active = false;
  }

  public getLabel(): string {
    return this.value;
  }
}