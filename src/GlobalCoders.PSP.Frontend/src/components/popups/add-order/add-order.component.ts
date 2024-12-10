import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsComponent } from '../../products/products.component';

@Component({
  selector: 'app-add-order',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './add-order.component.html',
  styleUrl: './add-order.component.css'
})
export class AddOrderComponent {
  isPopupVisible = false; // Controla si el popup está visible
  merchants: String[] = []; // Lista de comerciantes
  services: String[] = []; // Lista de servicios
  products: String[] = []; // Lista de productos
  selectedProduct: ProductsComponent | undefined; // Producto seleccionado
  selectedProducts: ProductsComponent[] = []; // Lista de productos seleccionados

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }
}
