import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-discount',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-discount.component.html',
  styleUrl: './edit-discount.component.css'
})
export class EditDiscountComponent {

  isPopupVisible = false;

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }
}
