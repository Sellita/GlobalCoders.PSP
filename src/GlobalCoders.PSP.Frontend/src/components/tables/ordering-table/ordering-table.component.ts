import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ordering-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ordering-table.component.html',
  styleUrl: './ordering-table.component.css'
})
export class OrderingTableComponent {
  @Input() columns: string[] = []; // Recibe los nombres de las columnas
  @Input() data: any[] = []; // Recibe los datos para las filas

  addRow() {
    this.data.push({});
  }

  deleteRow(index: number) {
    this.data.splice(index, 1);
  }
  
}
