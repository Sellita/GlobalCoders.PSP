import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { EventEmitter, Output } from '@angular/core';


@Component({
  selector: 'app-logout',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {

  @Output() messageEvent = new EventEmitter<string>();
  isPopupVisible = false;

  constructor(private authService: AuthService) {}

  openPopup() {
    this.isPopupVisible = true;
  }

  closePopup() {
    this.isPopupVisible = false;
  }

  onSubmit() {
    this.authService.logout();
    this.messageEvent.emit('Logged out');
  }

}
