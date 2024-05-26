import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {UserService} from "./services/user.service";
import {UserDTO} from "../generatedTypes/UserDTO";
import {map} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  constructor(private http: HttpClient, private userService: UserService)
  { }

  ngOnInit() {
  }

  logout(){
    this.userService.logout().then(r => console.log(r));
  }
}
