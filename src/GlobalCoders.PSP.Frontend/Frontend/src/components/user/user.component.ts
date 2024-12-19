
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { OrgService } from '../../services/org.service';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {

  edit: boolean = false;
  selectedUser: string = '';
  showForm: boolean = false;
  userForm: FormGroup;
  users: any[] = [];
  orgs: any[] = [];
  headers = ["ID","Name","Creation Time",  "Email", "Phone Number", "Role", "Status", "Merchant", "Working Schedule", "Actions"];
  days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

  constructor(private fb: FormBuilder, private orgService: OrgService, private userService: UserService) {
    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      organizationId: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]],
      role: ['', Validators.required],
      isActive: [null, Validators.required],
      workingSchedule: this.fb.array([]),
    });

    this.orgService.getOrganizations().subscribe((response: any) => {
      this.orgs = response.items;
    });

    this.loadEmployees();
  }

  get scheduleControls() {
    return this.userForm.get('schedule') as FormArray;
  }

  addSchedule() {
    const scheduleGroup = this.fb.group({
      dayOfWeek: [null, Validators.required],
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

  setStatus(status: boolean) {
    this.userForm.patchValue({ isActive: status });
  }

  onSubmit() {
    const userData = this.userForm.value;
    console.log('userData:', userData);
    if (this.edit) {
      const userIndex = this.users.findIndex(user => user.id === this.selectedUser);
      if (userIndex !== -1) {
      this.users[userIndex] = { ...userData, id: this.selectedUser };
      this.userService.updateEmployee(this.users[userIndex]).subscribe(
        (res) => {
        console.log('Empleado actualizado:', res);
        },
        (err) => {
        console.error('Error al actualizar el empleado:', err);
        }
      );
      }
      this.edit = false;
    } else {
      this.userService.createEmployee(userData).subscribe(
      (res: any) => {
        console.log('Nuevo empleado añadida:', res);
        this.users.push(res);
      },
      (err) => {
        console.error('Error al añadir el empleado:', err);
      }
      );
    }
    this.userForm.reset();
    this.scheduleControls.clear();
    this.addSchedule();
    this.showForm = false;
  }

  toggleForm(): void {
    this.showForm = !this.showForm;
    this.userForm.reset();
    this.edit = false;
    
  }

  loadEmployees() {
    this.userService.getEmployees().subscribe({
      next: (response: any) => {
        this.users = response.items;
        console.log('Empleados cargados:', this.users);
      },
      error: (err) => {
        console.error('Error al cargar los empleados:', err);
      }
    });
  }
  
  deleteUser(id: string) {
      this.userService.deleteEmployee(id).subscribe(
        (res) => {
          console.log('Empleado eliminado:', res);
          this.users = this.users.filter(org => org.id !== id);
        },
        (err) => {
          console.error('Error al eliminar el empleado:', err);
        }
      );
    }
  
  updateUser(user: User) {
    this.edit = true;
    this.selectedUser = user.id;
    let schedules: any[] = [];

    if (this.scheduleControls && user.workingSchedule) {
      schedules = user.workingSchedule.map((control: any) =>({
        dayOfWeek: control.dayOfWeek,
        startTime: control.startTime,
        endTime: control.endTime
      }));
    }


    this.userForm.patchValue({
        name: user.name,
        email: user.email,
        phoneNumber: user.phoneNumber,
        role: user.role,
        isActive: user.isActive,
        organizationId: this.orgService.getOrganization(user.organizationId).displayName,
        workingSchedule: schedules
    });

    this.showForm = true;
  }

}
