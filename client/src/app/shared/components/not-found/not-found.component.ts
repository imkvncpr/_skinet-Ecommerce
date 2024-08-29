import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [CommonModule, MatSnackBarModule,
    MatIcon, MatButton, RouterLink
  ],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss'
})
export class NotFoundComponent implements OnInit {
  constructor(private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.showNotFoundMessage();
  }

  showNotFoundMessage() {
    this.snackBar.open('Not Found', 'Close', {
      duration: 5000, 
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: ['not-found-snackbar']
    });
  }
}