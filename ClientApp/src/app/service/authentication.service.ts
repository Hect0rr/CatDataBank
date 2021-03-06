import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { User } from '../gateway/CdbService/model/user';
import { PublicationService } from 'src/app/service/publication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  isSessionActive: boolean = false
  user: User
  constructor(private httpClient: HttpClient, private router: Router, private pubService: PublicationService) {
    this.user = this.getSessionStorage()
  }

  getSessionStorage(): User {
    return JSON.parse(localStorage.getItem('user'))
  }

  postLogin(email: string, password: string) {
    console.log(email)
    return this.httpClient.post<boolean>("api/v1/auth", {
      "email": email,
      "password": password
    })
  }

  logout() {
    localStorage.clear()
    this.pubService.setLogText("Se Connecter")
  }

}
