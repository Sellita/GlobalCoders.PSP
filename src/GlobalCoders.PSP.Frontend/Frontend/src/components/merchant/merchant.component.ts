import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-merchant',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './merchant.component.html',
  styleUrl: './merchant.component.css'
})
export class MerchantComponent {

  showForm: boolean = false;
  companyForm: FormGroup;
  organizations: any[] = [];

  daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

  constructor(private fb: FormBuilder) {
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
      console.log('Nueva organización añadida:', this.companyForm.value);

      this.companyForm.reset();
      this.workingSchedule.clear();
      this.addWorkingSchedule(); // Reinicia con un horario vacío
    }
  }

  deleteOrganization(index: number) {
    this.organizations.splice(index, 1);
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

}
