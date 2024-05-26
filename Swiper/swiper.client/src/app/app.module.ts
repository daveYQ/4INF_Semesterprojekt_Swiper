import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import {BrowserModule, HammerGestureConfig} from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SwipeComponent } from './swipe/swipe.component';
import {NgOptimizedImage} from "@angular/common";
import { ProfileViewComponent } from './profile-view/profile-view.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { LoginComponent } from './login/login.component';
import {MatError} from "@angular/material/form-field";
import { MatCardModule} from "@angular/material/card";
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';  // Ensure this is imported
import { MatIconModule } from '@angular/material/icon';             // Optionally, if you use icons
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RouterOutlet} from "@angular/router";
import {MatMenu, MatMenuItem, MatMenuTrigger} from "@angular/material/menu";
import { RegisterComponent } from './register/register.component';

export class MyHammerConfig extends HammerGestureConfig {
  overrides = {
    'pan': { direction: Hammer.DIRECTION_HORIZONTAL },
  };
}

@NgModule({
  declarations: [
    AppComponent,
    SwipeComponent,
    LoginComponent,
    RegisterComponent
  ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        RouterOutlet,
        AppRoutingModule,
        NgOptimizedImage,
        MatInputModule,
        MatButtonModule,
        MatCardModule,
        MatError,
        MatFormFieldModule,  // Ensure this is imported
        MatIconModule,       // Optionally, if you use icons
        ReactiveFormsModule,
        MatMenu,
        MatMenuTrigger,
        HttpClientModule,
        FormsModule,
        MatMenuItem,
        // Ensure this is imported
    ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
