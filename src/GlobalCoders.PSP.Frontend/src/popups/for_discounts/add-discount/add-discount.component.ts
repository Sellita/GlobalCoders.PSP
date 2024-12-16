import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-discount',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './add-discount.component.html',
  styleUrl: './add-discount.component.css'
})
export class AddDiscountComponent {

  isPopupVisible = false;

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }
}
