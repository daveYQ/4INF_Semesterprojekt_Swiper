import { Component } from '@angular/core';
import {UserService} from "../user.service";
import {User} from "../User";

@Component({
  selector: 'app-swipe',
  templateUrl: './swipe.component.html',
  styleUrl: './swipe.component.css'
})
export class SwipeComponent {
  users: User[] = [];

  constructor(userService :UserService) {
    this.users = userService.getUsers();
  }
}
