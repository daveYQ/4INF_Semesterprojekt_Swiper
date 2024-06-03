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

  getAllUsers() {
    console.log("GETUSERS")
      return this.http.get<UserDTO[]>(this.url + '/User')
  }

  async login(email: string, password: string, options?: any) {
    console.log('email:', email);
    console.log('pwd:', password);

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

    return firstValueFrom(this.http.get<UserDTO>(this.url + '/User/LogIn', requestOptions));
  }

  async register(user: UserCreationDTO, options?: any)
  {
    console.log(user);
    return await firstValueFrom(this.http.post(this.url + "/User/Register", user, options));
  }

  async getCurrent(): Promise<UserDTO> {
    let opts =
      {
        withCredentials: true
      };

    try {
      const user = await firstValueFrom(this.http.get<UserDTO>(this.url + '/User/CurrentUser', opts));
      console.info(user);
      return user;
    } catch (error) {
      console.error(error);
      throw error; // Or handle the error appropriately
    }
  }

  async logout() {
    let opts =
      {
        withCredentials: true
      };

    try {
      await firstValueFrom(this.http.post(this.url + '/User/LogOff', {}, opts));
      console.info("Logged out");
    } catch (error) {
      console.error(error);
      throw error; // Or handle the error appropriately
    }
  }

  uploadImg(file: File)
  {
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post(this.url + "/User/ProfilePicture", formData, {
      reportProgress: true,
      observe: 'events',
      withCredentials: true
    });
  }

  likeUser(id: string)
  {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    let opts =
      {
        params: new HttpParams()
          .set('id', id),
        withCredentials: true
      };

    // Merge headers with options if provided
    const requestOptions = {
      headers: headers,
      ...opts
    };

    return this.http.post(this.url + "/User/ProfilePicture", null, requestOptions);
  }
}
