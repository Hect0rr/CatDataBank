import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PublicationService {
  private logText = new BehaviorSubject<String>("Se Connecter");
  private logText$ = this.logText.asObservable();
  constructor() { }


  setLogText(logText: String) {
    this.logText.next(logText);
  }

  getLogText(): Observable<String> {
    return this.logText$;
  }
}
