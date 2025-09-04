import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePickerModule } from "primeng/datepicker";
import { Calendar, LucideAngularModule } from "lucide-angular";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-date-select',
  imports: [DatePickerModule, LucideAngularModule, FormsModule, CommonModule],
  templateUrl: './task-date-select.html',
  styleUrl: './task-date-select.css'
})
export class TaskDateSelect {
  readonly Calendar = Calendar

  minDate = new Date();
  @Input() value: Date = new Date()
  @Output() onSelect = new EventEmitter<Date>()

  constructor() {
    this.minDate.setHours(0, 0, 0, 0)
  }

  onDateSelect(newDate: Date) {
    this.onSelect.emit(newDate)
  }
}
