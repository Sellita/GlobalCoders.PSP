import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-add-order',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './add-order.component.html',
  styleUrl: './add-order.component.css'
})

export class AddOrderComponent implements OnInit {
  
  isPopupVisible = false; // Controla si el popup está visible
  merchants: String[] = ["Barber","Coffee","Beauty"]; // Lista de comerciantes
  services: String[] = []; // Lista de servicios
  pizza: Product = {id: 0, name: "Pizza", price: 0, description: "", stock: 0, image: "", category: "", taxes: []}; // Producto
  products: Product[] = [this.pizza]; // Lista de productos
  selectedProduct: Product | undefined;
  selectedProducts: Product[] = [];
  quantity: number = 0;
  totalprice: number = 0;
  data: Order[] = []; // Lista de orden

  constructor() { }
  ngOnInit(): void {
  }



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

  addOrder(): Order{
    const newOrder: Order = {
      ID: this.data.length + 1,
      Date: new Date(),
      Client: "Client",
      Merchant: "Merchant",
      Service: "Service",
      "Nº of people": 0,
      Products: this.selectedProducts,
      Price: this.totalprice,
      Status: "Status"
    };
    this.data.push(newOrder);
    return newOrder;
  }

}

