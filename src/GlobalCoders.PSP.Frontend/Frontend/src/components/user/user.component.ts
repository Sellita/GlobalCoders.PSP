
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { OrgService } from '../../services/org.service';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {

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
  }

  ngOnInit(): void {

    // Suscribirse a la lista de servicios reactiva
    this.userService.users$.subscribe((data) => {
      this.users = data;
    });

    this.userService.getUsers().subscribe();

    this.orgService.getOrganizations().subscribe((data: any) => {
      this.orgs = data.items;
    });

    this.loadEmployees();

  }

  get scheduleControls() {
    return this.userForm.get('workingSchedule') as FormArray;
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


  loadEmployees() {
    this.userService.getUsers().subscribe({
      next: (response: any) => {
        this.users = response.items;
        console.log('Empleados cargados:', this.users);
      },
      error: (err) => {
        console.error('Error al cargar los empleados:', err);
      }
    });
  }
  
 
  toggleForm() {
      this.showForm = !this.showForm;
      this.edit = false;
      this.userForm.reset();
    }
  
    onSubmit() {
      if (this.userForm.valid) {
        console.log('Formulario válido:', this.userForm.value);
        if (this.edit) {
          this.userForm.patchValue({ employeeId: this.userForm.value.id });
          this.userService.updateUser(this.userForm.value).subscribe((data: any) => {
            console.log('User actualizado:', data);
          });
          this.edit = false;
          this.showForm = false;
        } else {
          this.userService.createUser(this.userForm.value).subscribe((data: any) => {
            console.log('User creado:', data);
          });
          this.showForm = false;
        }
      } else {
        alert('Por favor corrige los errores antes de enviar el formulario.');
      }
    }
  
    deleteUser(id: string) {
      this.userService.deleteUser(id).subscribe((data: any) => {
        console.log('User eliminado:', data);
      });
    }
  
    updateUser(user: User) {
      this.edit = true;
      this.showForm = true;
      this.userForm.patchValue(user);
    }
    

}
