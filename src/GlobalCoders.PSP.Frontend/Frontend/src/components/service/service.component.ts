import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ServiceService } from '../../services/service.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Service } from '../../models/service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-service',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './service.component.html',
  styleUrl: './service.component.css'
})
export class ServiceComponent {

  editMode: boolean = false;
  employees: any[] = [];
  showForm: boolean = false;
  serviceForm: FormGroup;
  services: Service[] = [];
  serviceStates = [
    { value: 0, label: 'Inactive' },
    { value: 1, label: 'Active' }  
  ];
  headers: string[] = [
    'ID',
    'Display Name',
    'Description',
    'Duration (Minutes)',
    'Price ($)',
    'Service State',
    'Employee ID',
    'Employee Name',
    'Creation Date',
    'Actions'
  ];



  constructor(
    private service: ServiceService,
    private fb: FormBuilder,
    private userService: UserService
  ) {
    this.serviceForm = this.fb.group({
      displayName: ['', Validators.required],
      description: ['', [Validators.required, Validators.maxLength(200)]],
      durationMin: [null, [Validators.required, Validators.min(1)]],
      price: [null, [Validators.required, Validators.min(0)]],
      serviceState: [1, Validators.required],
      employeeId: ['', Validators.required],
      id: ['']
    });
  }

  ngOnInit(): void {

    // Suscribirse a la lista de servicios reactiva
    this.service.services$.subscribe((data) => {
      this.services = data;
    });

    // Cargar empleados
    this.userService.getUsers().subscribe((data: any) => {
      this.employees = data.items || [];
    });

    // Cargar los servicios inicialmente
    this.service.getServices().subscribe();
  }


  toggleForm() {
    this.showForm = !this.showForm;
    this.editMode = false;
    this.serviceForm.reset();
  }

  submitForm() {
    if (this.serviceForm.valid) {
      if (this.editMode) {
        this.serviceForm.patchValue({ id: this.serviceForm.value.id });
        this.service.updateService(this.serviceForm.value).subscribe((data: any) => {
          console.log('Servicio actualizado:', data);
        });
        this.editMode = false;
        this.showForm = false;
      } else {
        this.service.createService(this.serviceForm.value).subscribe((data: any) => {
          console.log('Servicio creado:', data);
        });
        this.showForm = false;
      }
    } else {
      alert('Por favor corrige los errores antes de enviar el formulario.');
    }
  }

  deleteService(id: string) {
    this.service.deleteService(id).subscribe((data: any) => {
      console.log('Servicio eliminado:', data);
    });
  }

  updateService(service: Service) {
    this.editMode = true;
    this.showForm = true;
    this.serviceForm.patchValue(service);
  }

}
