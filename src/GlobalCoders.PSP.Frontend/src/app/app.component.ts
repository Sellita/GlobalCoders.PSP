import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../components/navbar/navbar.component';
import { AuthService } from '../services/auth.service';
import e from 'express';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent {
  title = 'GlobalCoders.PSP.Frontend';

  constructor(private authService: AuthService) {}


  ngOnInit() {
    if (!this.authService.isloggedIn()) {
      console.log('No está logueado');
    }else {
      console.log('Está logueado');
    }
  }

}
