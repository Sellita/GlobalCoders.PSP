import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  employee: string = 'Juan Perez';
  fecha: string = '';
  hora: string = '';
  revenue: number = 1000000;
  refunded: number = 10000;
  out_of_stock: number = 100;
  total_sales: number = 1000;
  messages: number = 10;
  reservations: number = 5;
  isloggedIn: boolean = false;

  constructor(private auth: AuthService) { }

  ngOnInit(): void {
    this.getCurrentDateTime();
    this.isloggedIn = this.auth.isloggedIn();
  }

  getCurrentDateTime() {
    const now = new Date();
    const hours = now.getHours();
    const minutes = now.getMinutes();
    const day = now.getDate();
    const month = now.getMonth() + 1; // Months are zero-based
    const year = now.getFullYear();
    this.fecha = `${day}/${month}/${year}`;
    this.hora = `${hours}:${minutes}`;
  }



}
