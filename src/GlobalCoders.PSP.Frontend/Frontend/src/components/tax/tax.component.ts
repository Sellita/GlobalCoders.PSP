import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ServiceService } from '../../services/service.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TaxService } from '../../services/tax.service';
import { OrgService } from '../../services/org.service';
import { Tax } from '../../models/tax';
import { Org } from '../../models/org';

@Component({
  selector: 'app-tax',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './tax.component.html',
  styleUrl: './tax.component.css'
})
export class TaxComponent {

  editMode: boolean = false;
  merchants: any[] = [];
  showForm: boolean = false;
  taxForm: FormGroup;
  taxes: Tax[] = [];

  taxTypes = [
      { value: 0, label: 'Percentage' },
      { value: 1, label: 'Value' }  
  ];
  taxStates = [
      { value: 0, label: 'Inactive' },
      { value: 1, label: 'Active' }  
  ];

  headers: string[] = [
      'ID',
      'Name',
      'Type',
      'Value',
      'Status',
      'Product ID',
      'Product Name',
      'Creation Date',
      'Merchant ID',
      'Merchant Name',
      'Actions'
  ];
  
  
    constructor(
      private taxService: TaxService,
      private fb: FormBuilder,
      private orgService: OrgService
    ) {
      this.taxForm = this.fb.group({
        name: ['', Validators.required],
        type: [0, Validators.required],
        value: [null, [Validators.required, Validators.min(0)]],
        status: [1, Validators.required],
        organizationId: ['', Validators.required],
        productId: ['', Validators.required],
        id: ['']
      });
    }
  
    ngOnInit(): void {
      // Suscribirse a la lista de servicios reactiva
      this.taxService.services$.subscribe((data) => {
        this.taxes = data;
      });
  
      // Cargar empleados
      this.orgService.getOrganizations().subscribe((data: any) => {
        this.merchants = data.items || [];
      });
  
      // Cargar los servicios inicialmente
      this.taxService.getTaxes().subscribe();
    }
  
  
    toggleForm() {
      this.showForm = !this.showForm;
      this.editMode = false;
      this.taxForm.reset();
    }
  
    submitForm() {
      if (this.taxForm.valid) {
        if (this.editMode) {
          this.taxForm.patchValue({ id: this.taxForm.value.id });
          this.taxService.updateTax(this.taxForm.value).subscribe((data: any) => {
            console.log('Tasa actualizada:', data);
          });
          this.editMode = false;
        } else {
          this.taxService.createTax(this.taxForm.value).subscribe((data: any) => {
            console.log('Tasa creada:', data);
          });
        }
      } else {
        alert('Por favor corrige los errores antes de enviar el formulario.');
      }
    }
  
    deleteTax(id: string) {
      this.taxService.deleteTax(id).subscribe((data: any) => {
        console.log('Tasa eliminada:', data);
      });
    }
  
    updateTax(tax: Tax) {
      this.editMode = true;
      this.showForm = true;
      this.taxForm.patchValue(tax);
    }

}
