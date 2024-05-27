import { Injectable } from '@angular/core';
import {User} from "../User";
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpParams} from "@angular/common/http";
import {UserCreationDTO} from "../../generatedTypes/UserCreationDTO";
import {catchError, firstValueFrom, map, throwError} from "rxjs";
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

  async getAllUsers(): Promise<UserDTO[]> {
    console.log("yay");

    try {
      const users = await firstValueFrom(this.http.get<UserDTO[]>(this.url + '/User'));
      console.info(users);
      return users;
    } catch (error) {
      console.error(error);
      throw error; // Or handle the error appropriately
    }
  }

  async login(email: string, password: string, options?: any) {
    console.log('email:', email);
    console.log('pwd:', password);

    // Create the body of the POST request
    const body = {
    };

    // Set headers if necessary
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    let opts =
      {
        params: new HttpParams()
          .set('email', email)
          .set('password', password)
          .set('rememberMe', true),
        withCredentials: true
      };

    // Merge headers with options if provided
    const requestOptions = {
      headers: headers,
      ...opts
    };

    console.log(body);
    let user = firstValueFrom(this.http.post<UserDTO>(this.url + '/User/LogIn', body, requestOptions));

    return user;
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
