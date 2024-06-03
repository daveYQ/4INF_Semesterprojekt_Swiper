import { Component, OnInit, AfterViewInit, Input, ViewChildren, QueryList, ElementRef, EventEmitter, Output, Renderer2 } from '@angular/core';
import {UserService} from "../services/user.service";
import {User} from "../User";
import {Subject} from "rxjs";
import {UserDTO} from "../../generatedTypes/UserDTO";
import {DomSanitizer} from "@angular/platform-browser";
//https://stackblitz.com/edit/ionic-4-template-czmv63?file=src%2Fapp%2Ftinder-ui-component%2Ftinder-ui.component.html
@Component({
  selector: 'app-swipe',
  templateUrl: './swipe.component.html',
  styleUrl: './swipe.component.css'
})
export class SwipeComponent implements OnInit, AfterViewInit{
  users: UserDTO[];

  @ViewChildren('tinderCard') tinderCards: QueryList<ElementRef>;
  tinderCardsArray: Array<ElementRef>;

  @Output() choiceMade = new EventEmitter();

  moveOutWidth: number;
  shiftRequired: boolean;
  transitionInProgress: boolean;
  heartVisible: boolean;
  crossVisible: boolean;

  constructor(private renderer: Renderer2, private userService: UserService, protected sanitizer: DomSanitizer) {
  }

  ngOnInit(){
    this.userService.getAllUsers().subscribe((res) => {
      this.users = res;
    })


  }

  userClickedButton(event, heart) {
    event.preventDefault();
    if (!this.users.length) return false;
    if (heart) {
      this.renderer.setStyle(this.tinderCardsArray[0].nativeElement, 'transform', 'translate(' + this.moveOutWidth + 'px, -100px) rotate(-30deg)');
      this.toggleChoiceIndicator(false,true);
      this.emitChoice(heart, this.users[0]);
    } else {
      this.renderer.setStyle(this.tinderCardsArray[0].nativeElement, 'transform', 'translate(-' + this.moveOutWidth + 'px, -100px) rotate(30deg)');
      this.toggleChoiceIndicator(true,false);
      this.emitChoice(heart, this.users[0]);
    };
    this.shiftRequired = true;
    this.transitionInProgress = true;
  };

  handlePan(event) {

    if (event.deltaX === 0 || (event.center.x === 0 && event.center.y === 0) || !this.users.length) return;

    if (this.transitionInProgress) {
      this.handleShift();
    }

    this.renderer.addClass(this.tinderCardsArray[0].nativeElement, 'moving');

    if (event.deltaX > 0) { this.toggleChoiceIndicator(false,true) }
    if (event.deltaX < 0) { this.toggleChoiceIndicator(true,false) }

    let xMulti = event.deltaX * 0.03;
    let yMulti = event.deltaY / 80;
    let rotate = xMulti * yMulti;

    this.renderer.setStyle(this.tinderCardsArray[0].nativeElement, 'transform', 'translate(' + event.deltaX + 'px, ' + event.deltaY + 'px) rotate(' + rotate + 'deg)');

    this.shiftRequired = true;

  };

  handlePanEnd(event) {
    this.toggleChoiceIndicator(false,false);

    if (!this.users.length) return;

    this.renderer.removeClass(this.tinderCardsArray[0].nativeElement, 'moving');

    let keep = Math.abs(event.deltaX) < 80 || Math.abs(event.velocityX) < 0.5;
    if (keep) {

      this.renderer.setStyle(this.tinderCardsArray[0].nativeElement, 'transform', '');
      this.shiftRequired = false;

    } else {

      let endX = Math.max(Math.abs(event.velocityX) * this.moveOutWidth, this.moveOutWidth);
      let toX = event.deltaX > 0 ? endX : -endX;
      let endY = Math.abs(event.velocityY) * this.moveOutWidth;
      let toY = event.deltaY > 0 ? endY : -endY;
      let xMulti = event.deltaX * 0.03;
      let yMulti = event.deltaY / 80;
      let rotate = xMulti * yMulti;

      this.renderer.setStyle(this.tinderCardsArray[0].nativeElement, 'transform', 'translate(' + toX + 'px, ' + (toY + event.deltaY) + 'px) rotate(' + rotate + 'deg)');

      this.shiftRequired = true;

      this.emitChoice(!!(event.deltaX > 0), this.users[0]);
    }
    this.transitionInProgress = true;
  };

  toggleChoiceIndicator(cross, heart) {
    this.crossVisible = cross;
    this.heartVisible = heart;
  };

  handleShift() {
    this.transitionInProgress = false;
    this.toggleChoiceIndicator(false,false)
    if (this.shiftRequired) {
      this.shiftRequired = false;
      this.users.shift();
    };
  };

  emitChoice(heart, card) {
    this.choiceMade.emit({
      choice: heart,
      payload: card
    })
  };

  ngAfterViewInit() {
    this.moveOutWidth = document.documentElement.clientWidth * 1.5;
    this.tinderCardsArray = this.tinderCards.toArray();
    this.tinderCards.changes.subscribe(()=>{
      this.tinderCardsArray = this.tinderCards.toArray();
    })
  };
}
