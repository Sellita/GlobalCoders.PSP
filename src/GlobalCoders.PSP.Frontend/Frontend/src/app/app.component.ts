import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';
import { NotificationComponent } from '../components/notification/notification.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule, NotificationComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {

  isloggedIn: boolean = false;

  constructor(private auth: AuthService) { }

  ngOnInit(): void {
    console.log(this.isloggedIn);
    this.isloggedIn = this.auth.isloggedIn();
  }

  logout() {
    this.auth.logout();
  }

}
