import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-edit-order',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-order.component.html',
  styleUrl: './edit-order.component.css'
})
export class EditOrderComponent {
  isPopupVisible = false; // Controla si el popup está visible
  merchants: String[] = ["Barber","Coffee","Beauty"]; // Lista de comerciantes
  services: String[] = []; // Lista de servicios
  pizza: Product = {id: 0, name: "Pizza", price: 0, description: "", stock: 0, image: "", category: "", taxes: []}; // Producto
  products: Product[] = [this.pizza]; // Lista de productos
  selectedProduct: Product | undefined;
  selectedProducts: Product[] = [];
  quantity: number = 0;
  totalprice: number = 0;

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }

  increment(){
    this.quantity+=1;

    if(this.selectedProduct){
      this.selectedProducts.push(this.selectedProduct);
      this.totalprice += this.selectedProduct.price * this.quantity;
    }
  }

  decrement(){
    if (this.quantity > 0){
      this.quantity--;
    }

    if(this.selectedProduct){
      this.selectedProducts.pop();
      this.totalprice -= this.selectedProduct.price * this.quantity;
    }
  }
}
