import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'MUNity';

  public constructor(private titleService: Title) {
    titleService.setTitle('MUNity');
  }

  setTitle(val: string) {
    this.titleService.setTitle(val);
  }
}
