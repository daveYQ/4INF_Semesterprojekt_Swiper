import { Component, Input, OnInit } from '@angular/core';
import {User} from "../User";
import {UserService} from "../services/user.service";
import {Subject} from "rxjs";
import {animate, keyframes, transition, trigger} from "@angular/animations";
import * as kf from './keyframes';

//https://stackblitz.com/edit/angular-tinder-swipe?file=src%2Fapp%2Fcard%2Fkeyframes.ts,src%2Fapp%2Fcard%2Fcard.component.html
@Component({
  selector: 'app-profile-view',
  templateUrl: './profile-view.component.html',
  styleUrl: './profile-view.component.css',
  animations: [
    trigger('cardAnimator', [
      transition('* => swiperight', animate(750, keyframes(kf.swiperight))),
      transition('* => swipeleft', animate(750, keyframes(kf.swipeleft)))
    ])
  ]
})
export class ProfileViewComponent {
  public users: User[] = [];
  public index = 0;
  @Input()
  parentSubject: Subject<any> = new Subject<any>();

  animationState: any;

  constructor(userService :UserService) {
    this.users = userService.getUsers();
  }

  ngOnInit() {
    this.parentSubject.subscribe(event => {
      this.startAnimation(event)
    });
  }

  startAnimation(state: any) {
    console.log("startani");
    if (!this.animationState) {
      console.log("inani")
      this.animationState = state;
    }
  }

  resetAnimationState(state: any) {
    this.animationState = '';
    this.index++;
  }


  ngOnDestroy() {
    this.parentSubject.unsubscribe();
  }


}
