import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { OrgService } from '../../services/org.service';
import { Org } from '../../models/org';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-merchant',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './merchant.component.html',
  styleUrl: './merchant.component.css'
})
export class MerchantComponent implements OnInit {

  
  headers = [
    "ID",
    "Display Name", 
    "Legal Name", 
    "Address", 
    "Email", 
    "Main Phone Number", 
    "Secondary Phone Number", 
    "Working Schedule", 
    "Actions"];
  daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
  editMode: boolean = false;
  showForm: boolean = false;
  companyForm: FormGroup;
  orgs: Org[] = [];

  constructor(
    private orgService: OrgService,
    private fb: FormBuilder,
  ) {
    this.companyForm = this.fb.group({
      displayName: ['', Validators.required],
      legalName: ['', Validators.required],
      address: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      mainPhoneNumber: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]],
      secondaryPhoneNumber: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]],
      workingSchedule: this.fb.array([]),
      id: ['']
    });
  }

  ngOnInit(): void {

    // Suscribirse a la lista de servicios reactiva
    this.orgService.organizations$.subscribe((data) => {
      this.orgs = data;
    });

    // Cargar los servicios inicialmente
    this.orgService.getOrganizations().subscribe(
      (data: any) => {
        data.items.forEach((org: Org) => {
          this.orgService.getOrganization(org.id).subscribe((data: any) => {
            this.orgs.push(data);
            console.log('organización cargada:', data);
          });
        });
      }
    );
  }

  get workingSchedule(): FormArray {
    return this.companyForm.get('workingSchedule') as FormArray;
  }

  addWorkingSchedule() {
    this.workingSchedule.push(
      this.fb.group({
        DayOfWeek: [null, Validators.required],
        StartTime: ['', Validators.required],
        EndTime: ['', Validators.required]
      })
    );
  }

  removeWorkingSchedule(index: number) {
    this.workingSchedule.removeAt(index);
  }

  toggleForm() {
    this.showForm = !this.showForm;
    this.editMode = false;
    this.companyForm.reset();
  }

  submitForm() {
    if (this.companyForm.valid) {
      if (this.editMode) {
        this.companyForm.patchValue({ id: this.companyForm.value.id });
        this.orgService.updateOrganization(this.companyForm.value).subscribe((data: any) => {
          console.log('Org actualizada:', data);
        });
        this.editMode = false;
        this.showForm = false;
      } else {
        this.orgService.createOrganization(this.companyForm.value).subscribe((data: any) => {
          console.log('Org creada:', data);
        });
        this.showForm = false;
      }
    } else {
      alert('Por favor corrige los errores antes de enviar el formulario.');
    }
  }

  deleteOrg(id: string) {
    this.orgService.deleteOrganization(id).subscribe((data: any) => {
      console.log('Org eliminada:', data);
    });
  }

  updateOrg(org: Org) {
    this.editMode = true;
    this.showForm = true;
    this.companyForm.patchValue(org);
  }

}
