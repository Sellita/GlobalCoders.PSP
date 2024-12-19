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

  edit: boolean = false;
  selectedOrg: string = '';

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
    this.addWorkingSchedule();

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


  submitForm() {

    const orgData = this.companyForm.value;
    if (this.edit) {
      const orgIndex = this.organizations.findIndex(org => org.id === this.selectedOrg);
      if (orgIndex !== -1) {
      this.organizations[orgIndex] = { ...orgData, id: this.selectedOrg };
      this.orgService.updateOrganization(this.organizations[orgIndex]).subscribe(
        (res) => {
        console.log('Organización actualizada:', res);
        },
        (err) => {
        console.error('Error al actualizar la organización:', err);
        }
      );
      }
      this.edit = false;
    } else {
      this.orgService.createOrganization(orgData).subscribe(
      (res: any) => {
        console.log('Nueva organización añadida:', res);
        this.organizations.push(res);
      },
      (err) => {
        console.error('Error al añadir la organización:', err);
      }
      );
    }
    this.companyForm.reset();
    this.workingSchedule.clear();
    this.addWorkingSchedule();
    this.showForm = false;
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
    this.edit = true;
    this.selectedOrg = org.id;

    this.companyForm.patchValue({
        displayName: org.displayName,
        legalName: org.legalName,
        address: org.address,
        email: org.email,
        mainPhoneNumber: org.mainPhoneNumber,
        secondaryPhoneNumber: org.secondaryPhoneNumber,
        workingSchedule: org.workingSchedule.map((schedule: any) => ({
            DayOfWeek: schedule.dayOfWeek,
            StartTime: schedule.startTime,
            EndTime: schedule.endTime,
        }))
    });

    this.showForm = true;

  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

}
