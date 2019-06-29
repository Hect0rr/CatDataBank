import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../gateway/CdbService/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthenticationService) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    // Get Session Token
    var user = this.authService.getSessionStorage()
    if (!user) {
      this.redirect(state)
      return false;
    }
    return true;
  }
  redirect(state: RouterStateSnapshot) {
    this.authService.logout()
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  }
}
