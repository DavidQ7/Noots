import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DialogsManageService } from 'src/app/content/navigation/dialogs-manage.service';
import { ExportService } from '../../export.service';
import { DocumentModel } from '../../models/ContentModel';
import { ParentInteraction } from '../../models/ParentInteraction.interface';

@Component({
  selector: 'app-document-note',
  templateUrl: './document-note.component.html',
  styleUrls: ['../styles/inner-card.scss'],
})
export class DocumentNoteComponent implements OnInit, ParentInteraction {
  @Input()
  content: DocumentModel;

  @Output() deleteDocumentEvent = new EventEmitter<string>();

  constructor(
    private dialogsManageService: DialogsManageService,
    private exportService: ExportService,
  ) {}

  setFocus = ($event?: any) => {
    console.log($event);
  };

  setFocusToEnd = () => {};

  updateHTML = (content: string) => {
    console.log(content);
  };

  getNative = () => {};

  getContent() {
    return this.content;
  }

  async exportDocument() {
    await this.exportService.exportDocument(this.content);
  }

  documentIcon() {
    const type = this.content.name.split('.').pop().toLowerCase();
    switch (type) {
      case 'doc':
      case 'docx':
        return 'microsoftWord';
      case 'xls':
      case 'xlsx':
        return 'microsoftExcel';
      case 'ppt':
      case 'pptx':
        return 'microsoftPowerpoint';
      case 'pdf':
        return 'pdf';
      default:
        return 'fileInner';
    }
  }

  openModal() {
    this.dialogsManageService.viewDock(this.content.documentPath);
  }

  mouseEnter = ($event: any) => {
    console.log($event);
  };

  mouseOut = ($event: any) => {
    console.log($event);
  };

  ngOnInit = () => {};
}
