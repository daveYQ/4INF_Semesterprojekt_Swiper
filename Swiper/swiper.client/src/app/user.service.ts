import { Injectable } from '@angular/core';
import {User} from "./User";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  users: User[] = [];
  constructor() {
    this.users.push(new User("Fixie Hartmann", 17));
    this.users.push(new User("Andi Arbeit", 18));
    this.users.push(new User("Hugo", 65));
  }

  getUsers()
  {
    return this.users;
  }
}
