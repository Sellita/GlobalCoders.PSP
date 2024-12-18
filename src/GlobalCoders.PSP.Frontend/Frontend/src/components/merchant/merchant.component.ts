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

  showForm: boolean = false;
  companyForm: FormGroup;
  organizations: Org[] = [];
  headers = ["ID","Display Name", "Legal Name", "Address", "Email", "Main Phone Number", "Secondary Phone Number", "Working Schedule", "Actions"];
  daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];


  constructor(private fb: FormBuilder, private orgService: OrgService) {
    this.companyForm = this.fb.group({
      displayName: ['', Validators.required],
      legalName: ['', Validators.required],
      address: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      mainPhoneNumber: ['', Validators.required],
      secondaryPhoneNumber: [''],
      workingSchedule: this.fb.array([])
    });

    this.addWorkingSchedule();
  }

  ngOnInit() {

    this.orgService.getOrganizations().subscribe(
      (res: any) => {
        res.items.forEach((org: Org) => {
          this.organizations.push(this.orgService.getOrganization(org.id));
        });
      },
      (err) => {
        console.error('Error al obtener las organizaciones:', err);
      }
    );
    console.log('Organizaciones:', this.organizations);
    this.addWorkingSchedule();

  }

  get workingSchedule(): FormArray {
    return this.companyForm.get('workingSchedule') as FormArray;
  }

  addWorkingSchedule() {
    this.workingSchedule.push(
      this.fb.group({
        DayOfWeek: [0, Validators.required],
        StartTime: ['', Validators.required],
        EndTime: ['', Validators.required]
      })
    );
  }

  removeWorkingSchedule(index: number) {
    this.workingSchedule.removeAt(index);
  }


  submitForm() {
    if (this.companyForm.valid) {
      this.organizations.push({ ...this.companyForm.value });
      this.orgService.createOrganization(this.companyForm.value).subscribe(
        (res) => {
          console.log('Nueva organización añadida:', res);
        },
        (err) => {
          console.error('Error al añadir la organización:', err);
        }
      );

      console.log('Nueva organización añadida:', this.companyForm.value);

      this.companyForm.reset();
      this.workingSchedule.clear();
      this.addWorkingSchedule(); // Reinicia con un horario vacío
    }
  }

  deleteOrg(id: string) {
    this.orgService.deleteOrganization(id).subscribe(
      (res) => {
        console.log('Organización eliminada:', res);
        this.organizations = this.organizations.filter(org => org.id !== id);
      },
      (err) => {
        console.error('Error al eliminar la organización:', err);
      }
    );
  }

  updateOrg(org: Org) {
    this.orgService.updateOrganization(org);
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

}
