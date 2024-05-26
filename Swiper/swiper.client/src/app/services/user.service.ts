import { Injectable } from '@angular/core';
import {User} from "../User";
import {HttpClient} from "@angular/common/http";
import {UserCreationDTO} from "../../generatedTypes/UserCreationDTO";
import {catchError, firstValueFrom, throwError} from "rxjs";
import {UserDTO} from "../../generatedTypes/UserDTO";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public readonly url: string = 'https://localhost:7281'

  users: User[] = [];
  constructor(private http: HttpClient) {
    this.users.push(new User("Fixie Hartmann", 17));
    this.users.push(new User("Andi Arbeit", 18));
    this.users.push(new User("Hugo", 65));
  }

  getUsers()
  {
    return this.users;
  }

  getAllUsers() {
      return this.http.get<UserDTO[]>(this.url + '/User')
  }

  async login(user: UserDTO, options?: any)
  {
    console.log(user);
    return this.http.post(this.url + "/User/Login", user, options).pipe(
      catchError((error) => {
        console.error('Error fetching data:', error);
        return throwError(() => error);
      })
    );
  }

  async register(user: UserCreationDTO, options?: any)
  {
    console.log(user);
    return this.http.post(this.url + "/User/Register", user, options).pipe(
      catchError((error) => {
        console.error('Error fetching data:', error);
        return throwError(() => error);
      })
    );
  }

  async getCurrent(): Promise<UserDTO> {
    try {
      const user = await firstValueFrom(this.http.get<UserDTO>(this.url + '/User/CurrentUser'));
      console.info(user);
      return user;
    } catch (error) {
      console.error(error);
      throw error; // Or handle the error appropriately
    }
  }

  async logout() {
    try {
      await firstValueFrom(this.http.post(this.url + '/User/LogOff', {}));
      console.info("Logged out");
    } catch (error) {
      console.error(error);
      throw error; // Or handle the error appropriately
    }
  }
}
