import { Component, ViewChild } from '@angular/core';
import { AddOrderComponent } from '../popups/add-order/add-order.component';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [AddOrderComponent],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {

  @ViewChild('popup')
  popup: AddOrderComponent = new AddOrderComponent;

  ngOnInit(): void {
  }

  addOrder() {
    this.popup.openPopup();
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
