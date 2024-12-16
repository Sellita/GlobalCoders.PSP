import { Component, ViewChild } from '@angular/core';
import { AddDiscountComponent } from '../../popups/for_discounts/add-discount/add-discount.component';
import { EditDiscountComponent } from '../../popups/for_discounts/edit-discount/edit-discount.component';

@Component({
  selector: 'app-discounts',
  standalone: true,
  imports: [AddDiscountComponent, EditDiscountComponent],
  templateUrl: './discounts.component.html',
  styleUrl: './discounts.component.css'
})

export class DiscountsComponent {

  @ViewChild('popup') popup: AddDiscountComponent = new AddDiscountComponent;
  @ViewChild('popupedit') popupedit: EditDiscountComponent = new EditDiscountComponent;
  
  ngOnInit(): void {
  }

  addDiscount() {
    console.log('Add order');
  }

  editDiscount() {
    console.log('Edit order');
  }

  deleteDiscount() {
    console.log('Delete order');
  }

  filterOrders() {
    console.log('Filter orders');
  }
}
