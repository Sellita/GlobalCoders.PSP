import { Component, ViewChild } from '@angular/core';
import { AddOrderComponent } from '../../popups/add-order/add-order.component';
import { EditOrderComponent } from '../../popups/edit-order/edit-order.component';
import { OrderingTableComponent } from '../tables/ordering-table/ordering-table.component';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [AddOrderComponent, EditOrderComponent, OrderingTableComponent],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})

export class OrdersComponent {

  @ViewChild('popup') popup: AddOrderComponent = new AddOrderComponent;
  @ViewChild('popupedit') popupedit: EditOrderComponent = new EditOrderComponent;
  @ViewChild('orderingTable') orderingTable: OrderingTableComponent = new OrderingTableComponent;
  columns = ["ID", "Date", "Client", "Merchant", "Service", "Nº of people", "Products", "Price", "Status"];
  data: Order[] = [];

  ngOnInit(): void {
    this.data = [this.popup.addOrder()];
  }

  addOrder() {
    this.popup.openPopup();
  }

  editOrder() {
    this.popupedit.openPopup();
  }

  deleteOrder() {
    console.log('Delete order');
  }

  filterOrders() {
    console.log('Filter orders');
  }

  createOrder() {
    this.orderingTable.addRow();
  }
}
