import {Component} from '@angular/core';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {FormControl, Validators, FormsModule, ReactiveFormsModule} from '@angular/forms';
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
export class LoginComponent {
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required, Validators.minLength(8)]);
  errorMessage: string;

  constructor(private userService: UserService, private router: Router) {
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
        console.log(user);
        this.router.navigate(['/'])
      });

      //console.log(req.subscribe());
  }
}
