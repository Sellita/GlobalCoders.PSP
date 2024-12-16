import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { EventEmitter, Output } from '@angular/core';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent {
    email: string = '';
    password: string = '';
    isPopupVisible = false;

    @Output() messageEvent = new EventEmitter<string>();

    constructor(private authService: AuthService) {}

    openPopup() {
      this.isPopupVisible = true;
    }

    closePopup() {
      this.isPopupVisible = false;
    }

    onSubmit() {
      this.authService.login(this.email, this.password).subscribe({
        next: data => {
          localStorage.setItem('accessToken', data.accessToken);
          localStorage.setItem('refreshToken', data.refreshToken);
          this.messageEvent.emit('Logged in');
          window.location.href = '/dashboard';
        },
        error: error => {
          console.error('Error:', error);
        }
      });
    }
  }
