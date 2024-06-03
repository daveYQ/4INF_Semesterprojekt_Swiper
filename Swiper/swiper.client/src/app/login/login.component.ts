import {Component, OnInit} from '@angular/core';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {FormControl, Validators, FormsModule, ReactiveFormsModule, FormGroup, FormBuilder} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {merge} from 'rxjs';
import {UserService} from "../services/user.service";
import {UserCreationDTO} from "../../generatedTypes/UserCreationDTO";
import {UserDTO} from "../../generatedTypes/UserDTO";
import {Router} from "@angular/router";
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  email: FormControl;
  password: FormControl;
  errorMessage: string;
  loginForm: FormGroup;
  apiError: string;

  constructor(private userService: UserService, private router: Router, private formBuilder: FormBuilder) {
  }

  ngOnInit()
  {
    this.email = new FormControl('', [Validators.required, Validators.email]);
    this.password = new FormControl('', [Validators.required, Validators.minLength(8)]);

    this.loginForm = this.formBuilder.group({
      email: this.email,
      password: this.password
    })
  }

  updateErrorMessage() {
    if (this.email.hasError('required')) {
      this.errorMessage = 'You must enter a value';
    } else if (this.email.hasError('email')) {
      this.errorMessage = 'Not a valid email';
    } else {
      this.errorMessage = '';
    }
  }

  login() {
      let email = this.email.value;
      let password = this.password.value;

      let res = this.userService.login(email, password);

      res.then(user =>
      {
        this.router.navigate(['/'])
      });

    try
    {
      this.userService.login(email, password).then(() => {
        this.router.navigate(['']);
      }).catch(err => this.apiError = err.error);
    } catch (err) {
      this.apiError = err.error;
    }
  }
}
