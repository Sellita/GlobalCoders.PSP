import { Component } from '@angular/core';

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [],
  templateUrl: './payments.component.html',
  styleUrl: './payments.component.css'
})
export class PaymentsComponent {
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
