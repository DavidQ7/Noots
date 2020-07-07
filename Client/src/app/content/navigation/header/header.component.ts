import { Component, OnInit } from '@angular/core';
import { PersonalizationService } from 'src/app/shared/services/personalization.service';
import { Theme } from 'src/app/shared/enums/Theme';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  theme = Theme;

  constructor(public pService: PersonalizationService) { }

  ngOnInit(): void {
  }

  toggleTheme() {
    if (this.pService.theme === Theme.Light) {
      this.pService.theme = Theme.Dark;
    } else {
      this.pService.theme = Theme.Light;
    }
  }

  toggleSidebar() {
    this.pService.stateSidebar = !this.pService.stateSidebar;
  }

}
