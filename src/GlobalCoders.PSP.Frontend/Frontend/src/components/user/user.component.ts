
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { OrgService } from '../../services/org.service';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {

  showForm: boolean = false;
  userForm: FormGroup;
  users: any[] = [];
  orgs: any[] = [];
  days = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];

  constructor(private fb: FormBuilder, private orgService: OrgService) {
    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      business: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]],
      role: ['', Validators.required],
      status: ['', Validators.required],
      schedule: this.fb.array([]),
    });

    this.orgService.getOrganizations().subscribe((response: any) => {
      this.orgs = response.items;
    });
  }

  get scheduleControls() {
    return this.userForm.get('schedule') as FormArray;
  }

  addSchedule() {
    const scheduleGroup = this.fb.group({
      day: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
    });
    this.scheduleControls.push(scheduleGroup);
  }

  removeSchedule(index: number) {
    this.scheduleControls.removeAt(index);
  }

  setRole(role: string) {
    this.userForm.patchValue({ role });
  }

  setStatus(status: string) {
    this.userForm.patchValue({ status });
  }

  onSubmit() {
    if (this.userForm.valid) {
      console.log('Formulario enviado', this.userForm.value);
    } else {
      console.log('Formulario inválido');
    }
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
  }

}
