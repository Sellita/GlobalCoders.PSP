import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-tax',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './add-tax.component.html',
  styleUrl: './add-tax.component.css'
})

export class AddTaxComponent {

  isPopupVisible = false;

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }
}
