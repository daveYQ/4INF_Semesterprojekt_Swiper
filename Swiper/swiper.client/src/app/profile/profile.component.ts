import { Component } from '@angular/core';
import {UserService} from "../services/user.service";
import {HttpClient, HttpEventType} from "@angular/common/http";
import {finalize, Subscription} from "rxjs";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  requiredFileType:string = "image/jpeg";

  fileName = '';
  uploadProgress:number;
  uploadSub: Subscription;

  constructor(private http: HttpClient, private userService: UserService) {}

  onFileSelected(event) {
    const file:File = event.target.files[0];

    if (file) {
      this.fileName = file.name;

      const upload$ = this.userService.uploadImg(file)
        .pipe(
          finalize(() => this.reset())
        );

      this.uploadSub = upload$.subscribe(event => {
        if (event.type == HttpEventType.UploadProgress) {
          this.uploadProgress = Math.round(100 * (event.loaded / event.total));
        }
      })
    }
  }

  cancelUpload() {
    this.uploadSub.unsubscribe();
    this.reset();
  }

  reset() {
    this.uploadProgress = null;
    this.uploadSub = null;
  }
}
