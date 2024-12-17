import { Component, ViewChild } from '@angular/core';
import { AddMerchantComponent } from '../../popups/for_merchants/add-merchant/add-merchant.component';
import { EditMerchantComponent } from '../../popups/for_merchants/edit-merchant/edit-merchant.component';
import { DeletePopupComponent } from '../../popups/delete-popup/delete-popup.component';
import { OrderingTableComponent } from '../tables/ordering-table/ordering-table.component';
import { OrganizationService } from '../../services/organization.service';
import { Organization } from '../../models/organization.model';

@Component({
  selector: 'app-merchants',
  standalone: true,
  imports: [AddMerchantComponent, EditMerchantComponent, DeletePopupComponent, OrderingTableComponent],
  templateUrl: './merchants.component.html',
  styleUrl: './merchants.component.css'
})
export class MerchantsComponent {

  selectedMerchant: any = null;
  @ViewChild('popup') popup: AddMerchantComponent = new AddMerchantComponent;
  @ViewChild('popupedit') popupedit: EditMerchantComponent = new EditMerchantComponent;
  @ViewChild('popupdelete') popupdelete: DeletePopupComponent = new DeletePopupComponent(this.selectedMerchant);
  @ViewChild('table') table: OrderingTableComponent = new OrderingTableComponent;
  columns = ["ID", "Display Name", "Legal Name", "Phone", "Second Phone", "Address", "Email", "Working Schedule"];
  data: Organization[] = [];
  
  constructor(private organizationService: OrganizationService) {}

  ngOnInit(): void {
    this.organizationService.getOrganizations().subscribe((data: any) => {
      data.items.forEach((item: any) => {
        this.organizationService.getOrganization(item.id).subscribe((org: any) => {
          console.log(org);
        });
      });
    });
  }

  addMerchant() {
    this.popup.openPopup();
  }

  editMerchant() {
    this.popupedit.openPopup();
  }

  deleteMerchant() {
    this.popupdelete.openPopup();
  }

  filterOrders() {
    console.log('Filter orders');
  }
}


