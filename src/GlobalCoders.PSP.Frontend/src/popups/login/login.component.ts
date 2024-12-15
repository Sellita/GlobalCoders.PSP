import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

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
    isPopupVisible = false; // Controla si el popup está visible

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
          window.location.href = '/dashboard';
        },
        error: error => {
          console.error('Error:', error);
        }
      });
    }
  }
