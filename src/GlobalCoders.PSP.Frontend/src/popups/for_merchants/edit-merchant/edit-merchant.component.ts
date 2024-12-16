import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-merchant',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-merchant.component.html',
  styleUrl: './edit-merchant.component.css'
})
export class EditMerchantComponent {

  isPopupVisible = false;

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }

}
