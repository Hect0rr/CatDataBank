import { Component } from '@angular/core';
import { AuthenticationService } from '../service/authentication.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { PublicationService } from '../service/publication.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  loginText: String;

  constructor(private pubService: PublicationService){}

  ngOnInit() {
    this.pubService.getLogText().subscribe(logText => this.loginText = logText);
  }

 

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
