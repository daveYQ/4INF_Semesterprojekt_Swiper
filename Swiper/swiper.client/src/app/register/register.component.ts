import {Component} from '@angular/core';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {FormControl, Validators, FormsModule, ReactiveFormsModule, FormGroup} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {merge} from 'rxjs';
import {UserService} from "../services/user.service";
import {UserCreationDTO} from "../../generatedTypes/UserCreationDTO";
import {UserDTO} from "../../generatedTypes/UserDTO";
import {LoginComponent} from "../login/login.component";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required, Validators.minLength(8)]);
  username = new FormControl('');
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

  async onClickSubmit() {
    let email = this.email.value;
    let username = this.username.value;
    let password = this.password.value;

    let user = new UserCreationDTO(username, email, password);

    console.log("test")

    this.userService.register(user).then(() => {
      console.log("o kurwa raketa")
      this.router.navigate(['']);
    });
  }
}
