import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Organization } from '../../../models/organization.model';

@Component({
  selector: 'app-add-merchant',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-merchant.component.html',
  styleUrl: './add-merchant.component.css'
})
export class AddMerchantComponent {

  isPopupVisible = false;
  merchant: Organization = {
    displayName: "",
    legalName: "",
    address: "",
    email: "",
    mainPhoneNumber: "",
    secondaryPhoneNumber: "",
    workingSchedule: [
      { DayOfWeek: 0, StartTime: '', EndTime: '' },
      { DayOfWeek: 1, StartTime: '', EndTime: '' },
      { DayOfWeek: 2, StartTime: '', EndTime: '' },
      { DayOfWeek: 3, StartTime: '', EndTime: '' },
      { DayOfWeek: 4, StartTime: '', EndTime: '' },
      { DayOfWeek: 5, StartTime: '', EndTime: '' },
      { DayOfWeek: 6, StartTime: '', EndTime: '' }   
    ],
    id: ""
  };
  weekDays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

  constructor(){}

  openPopup() {
    this.isPopupVisible = true; // Abre el popup
  }

  closePopup() {
    this.isPopupVisible = false; // Cierra el popup
  }

  createOrganization() {
    // Crea la organización
    console.log(this.merchant);
  }
}
