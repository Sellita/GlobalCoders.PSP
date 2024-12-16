import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.css'
})

export class AddProductComponent {

  isPopupVisible = false;

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }
}
