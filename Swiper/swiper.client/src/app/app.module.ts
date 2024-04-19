import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SwipeComponent } from './swipe/swipe.component';
import {NgOptimizedImage} from "@angular/common";
import { ProfileViewComponent } from './profile-view/profile-view.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

@NgModule({
  declarations: [
    AppComponent,
    SwipeComponent,
    ProfileViewComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, NgOptimizedImage
  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
