import { Component } from '@angular/core';
import {UserService} from "../user.service";
import {User} from "../User";
import {Subject} from "rxjs";

@Component({
  selector: 'app-swipe',
  templateUrl: './swipe.component.html',
  styleUrl: './swipe.component.css'
})
export class SwipeComponent {
  parentSubject:Subject<string> = new Subject();

  cardAnimation(value:string) {
    this.parentSubject.next(value);
  }
}
