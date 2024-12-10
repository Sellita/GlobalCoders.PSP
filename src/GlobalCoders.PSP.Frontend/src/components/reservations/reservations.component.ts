import { Component } from '@angular/core';

@Component({
  selector: 'app-reservations',
  standalone: true,
  imports: [],
  templateUrl: './reservations.component.html',
  styleUrl: './reservations.component.css'
})
export class ReservationsComponent {
  ngOnInit(): void {
  }

  addOrder() {
    console.log('Add order');
  }

  editOrder() {
    console.log('Edit order');
  }

  deleteOrder() {
    console.log('Delete order');
  }

  filterOrders() {
    console.log('Filter orders');
  }
}
