import { Component } from '@angular/core';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
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
