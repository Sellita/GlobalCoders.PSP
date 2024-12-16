import { Component, OnInit, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RouterLinkActive } from '@angular/router';
import { LoginComponent } from '../../popups/for_login/login/login.component';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { LogoutComponent } from '../../popups/for_login/logout/logout.component';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule, LoginComponent, LogoutComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {


  @ViewChild('login') popup!: LoginComponent;
  @ViewChild('logout') logoutpopup!: LogoutComponent;

  action: string = '';
  img_url: string = '';
  isloggedIn: boolean = false;
  message: string = '';

  constructor(private auth: AuthService, private userService: UserService) { }

  ngOnInit(): void {
    this.action = this.auth.isloggedIn() ? 'Log out' : 'Login';
    this.img_url = this.auth.isloggedIn() ? '/logout.png' : '/login.png';
    this.isloggedIn = this.auth.isloggedIn();
  }

  showloginpopup() {
    this.popup.openPopup();
  }

  handleMessage(message: string) {
    if (message === 'Logged in') {
      this.isloggedIn = true;
      this.action = 'Log out';
      this.img_url = '/logout.png';
    }else if (message === 'Logged out') {
      this.isloggedIn = false;
      this.action = 'Login';
      this.img_url = '/login.png';
      window.location.href = '/dashboard';
    }
  }

  showlogoutpopup() {
    this.logoutpopup.openPopup();
    this.userService.getUsers().subscribe((data) => {
      console.log(data);
    });
  }

}
