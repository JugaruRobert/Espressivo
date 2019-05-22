import { Component, Inject } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { UserProfileComponent } from './user-profile/user-profile.component';

export interface DialogData {
  animal: 'panda' | 'unicorn' | 'lion';
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Frontend';

  constructor(public dialog: MatDialog) {}

  openDialog() {
    this.dialog.open(UserProfileComponent, {
      data: {
        animal: 'panda'
      }
    });
  }
}
