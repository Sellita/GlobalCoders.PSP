import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { OrgService } from '../../services/org.service';
import { ProductService } from '../../services/product.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Product } from '../../models/product';
import { TaxService } from '../../services/tax.service';
import { ProductType } from '../../models/product-type';
import { ProductTypeService } from '../../services/product-type.service';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent {

    editMode: boolean = false;
    merchants: any[] = [];
    taxes: any[] = [];  
    showForm: boolean = false;
    productForm: FormGroup;
    productsTypes: ProductType[] = [];
    products: Product[] = [];
    productStates = [
      { value: 0, label: 'Inactive' },
      { value: 1, label: 'Active' }  
    ];
    headers: string[] = [
      'ID',
      'Name',
      'Description',
      'Stock',
      'Tax Name',
      'Tax Value',
      'Category ID',
      'Category',
      'Price',
      'Product State',
      'Merchant ID',
      'Merchant',
      'Creation Date',
      'Actions'
    ];
  
  
    constructor(
      private productService: ProductService,
      private fb: FormBuilder,
      private orgService: OrgService,
      private taxService: TaxService,
      private productTypeService: ProductTypeService
    ) {
      this.productForm = this.fb.group({
        displayName: ['', Validators.required],
        description: ['', [Validators.required, Validators.maxLength(200)]],
        stock: [null, [Validators.required, Validators.min(0)]],
        productTypeId: ['', Validators.required],
        price: [null, [Validators.required, Validators.min(0)]],
        productState: [1, Validators.required],
        merchantId: ['', Validators.required],
      });
    }
  
    ngOnInit(): void {
  
      // Suscribirse a la lista de servicios reactiva
      this.productService.products$.subscribe((data) => {
        this.products = data;
      });

      // Cargar merchants
      this.orgService.getOrganizations().subscribe((data: any) => {
        this.merchants = data.items || [];
      });

      //Cargar tasas
      this.taxService.getTaxes().subscribe((data: any) => {
        this.taxes = data.items || [];
      });

      //Cargar los product-types
      this.productTypeService.getProductsTypes().subscribe((data: any) => {
        this.productsTypes = data.items || [];
      });

      // Cargar los servicios inicialmente
      this.productService.getProducts().subscribe();
    }
  
  
    toggleForm() {
      this.showForm = !this.showForm;
      this.editMode = false;
      this.productForm.reset();
    }
  
    submitForm() {
      if (this.productForm.valid) {
        if (this.editMode) {
          const formValue = this.productForm.value;
          formValue.productState = Number(formValue.productState);
            formValue.id = this.productForm.value.id;
          this.productForm.patchValue({ id: this.productForm.value.id });
          this.productService.updateProduct(this.productForm.value).subscribe((data: any) => {
            console.log('Producto actualizado:', data);
          });
          this.editMode = false;
          this.showForm = false;
        } else {
          console.log('Formulario:', this.productForm.value);
          const formValue = this.productForm.value;
          formValue.productState = Number(formValue.productState);
          this.productService.createProduct(this.productForm.value).subscribe((data: any) => {
            console.log('Producto creado:', data);
          });
          this.showForm = false;
        }
      } else {
        alert('Por favor corrige los errores antes de enviar el formulario.');
      }
    }
  
    deleteService(id: string) {
      this.productService.deleteProduct(id).subscribe((data: any) => {
        console.log('Producto eliminado:', data);
      });
    }
  
    updateService(product: Product) {
      this.editMode = true;
      this.showForm = true;
      this.productForm.patchValue(product);
    }
  
}
