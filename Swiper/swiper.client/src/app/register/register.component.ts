import {Component, OnInit} from '@angular/core';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {FormControl, Validators, FormsModule, ReactiveFormsModule, FormGroup, FormBuilder} from '@angular/forms';
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
export class RegisterComponent implements OnInit{
  //email = new FormControl('', [Validators.required, Validators.email]);
  //password = new FormControl('', [Validators.required, Validators.minLength(8)]);
  //username = new FormControl('');
  errorMessage: string;
  apiError: string;
  registerForm: FormGroup;
  username: FormControl;
  email: FormControl;
  password: FormControl;
  submitted = false;

  constructor(private userService: UserService, private router: Router, private formBuilder: FormBuilder) {
  }

  ngOnInit()
  {
    this.username = new FormControl('', [Validators.required]);
    this.email = new FormControl('', [Validators.required, Validators.email]);
    this.password = new FormControl('', [Validators.required, Validators.minLength(8)]);

    this.registerForm = this.formBuilder.group({
      username: this.username,
      email: this.email,
      password: this.password
    })
  }

  updateErrorMessage() {
    if (this.registerForm.controls.email.hasError('required')) {
      this.errorMessage = 'Email required!';
    } else if (this.registerForm.controls.email.hasError('email')) {
      this.errorMessage = 'Not a valid email!';
    } else {
      this.errorMessage = '';
    }
  }

  async onClickSubmit() {
    this.submitted = true;

    let email = this.registerForm.get("email").value;
    let username = this.registerForm.get("username").value;
    let password = this.registerForm.get("password").value;


    let user = new UserCreationDTO(username, email, password);

    if(!this.registerForm.valid)
    {
      return;
    }

    try
    {
      this.userService.register(user).then(() => {
        this.router.navigate(['']);
      })
        .catch(err => this.apiError = err.error);
    } catch (err) {
      this.apiError = err.error;
    }
  }
}
