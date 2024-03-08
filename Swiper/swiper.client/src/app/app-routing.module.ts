import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SwipeComponent} from "./swipe/swipe.component";

const routes: Routes = [
  { path:'', component: SwipeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
