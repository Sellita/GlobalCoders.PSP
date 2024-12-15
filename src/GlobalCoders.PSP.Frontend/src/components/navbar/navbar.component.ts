import { Component, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RouterLinkActive } from '@angular/router';
import { LoginComponent } from '../../popups/login/login.component';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, LoginComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  @ViewChild('login') popup!: LoginComponent;

  showloginpopup() {
    this.popup.openPopup();
  }

  logout() {
    // logout
  }

}
