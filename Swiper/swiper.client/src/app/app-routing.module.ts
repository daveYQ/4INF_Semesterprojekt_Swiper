import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SwipeComponent} from "./swipe/swipe.component";
import {LoginComponent} from "./login/login.component";
import {RegisterComponent} from "./register/register.component";
import {authGuard} from "./services/auth.guard";

const routes: Routes = [
  { path:'', component: SwipeComponent, canActivate: [authGuard]},
  { path:'login', component: LoginComponent},
  { path:'register', component: RegisterComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
