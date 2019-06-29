import { Component, NgModule } from '@angular/core';
import { LoginComponent } from '../login/login.component';

@NgModule({
  imports: [LoginComponent],
  declarations: [],
  bootstrap: []
})

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor() { }

  ngOnInit() {
    
  }

}
