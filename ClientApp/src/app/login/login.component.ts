import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthenticationService } from '../service/authentication.service';
import { PublicationService } from '../service/publication.service';

@Component({
  templateUrl: 'login.component.html',
  selector: 'login-component'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthenticationService,
    private pubSevice: PublicationService) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    // reset login status
    this.authService.logout();

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }
    console.log("test")
    this.loading = true;
    this.authService.postLogin(this.f.username.value, this.f.password.value)
      .pipe(first())
      .subscribe(
        (data: any) => {
          localStorage.setItem('user', JSON.stringify(data))
          this.pubSevice.setLogText("Se Déconnecter")
          this.router.navigate([this.returnUrl]);
        },
        (error: any) => {
          this.loading = false;
          this.error = error;
        });
  }
}