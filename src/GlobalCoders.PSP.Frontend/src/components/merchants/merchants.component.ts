import { Component } from '@angular/core';

@Component({
  selector: 'app-merchants',
  standalone: true,
  imports: [],
  templateUrl: './merchants.component.html',
  styleUrl: './merchants.component.css'
})
export class MerchantsComponent {
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
