import { Component, ViewChild } from '@angular/core';
import { AddMerchantComponent } from '../../popups/for_merchants/add-merchant/add-merchant.component';
import { EditMerchantComponent } from '../../popups/for_merchants/edit-merchant/edit-merchant.component';

@Component({
  selector: 'app-merchants',
  standalone: true,
  imports: [AddMerchantComponent, EditMerchantComponent],
  templateUrl: './merchants.component.html',
  styleUrl: './merchants.component.css'
})
export class MerchantsComponent {

  @ViewChild('popup') popup: AddMerchantComponent = new AddMerchantComponent;
  @ViewChild('popupedit') popupedit: EditMerchantComponent = new EditMerchantComponent;

  ngOnInit(): void {
  }

  addMerchant() {
    this.popup.openPopup();
  }

  editMerchant() {
    this.popupedit.openPopup();
  }

  deleteMerchant() {
    console.log('Delete order');
  }

  filterOrders() {
    console.log('Filter orders');
  }
}


