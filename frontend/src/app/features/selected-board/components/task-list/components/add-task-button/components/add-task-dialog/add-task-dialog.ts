import { Component, Input, OnInit, signal } from '@angular/core';
import { Calendar, List, LucideAngularModule, Tag } from "lucide-angular";
import { Select } from "@app/shared/components/select/select";
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { SelectItem } from '@app/shared/components/select/models/select-item.model';
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { selectSelectedBoardColumns } from '@app/features/selected-board/store/selected-board.selectors';
import { CommonModule } from '@angular/common';
import { DatePickerModule } from 'primeng/datepicker';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-task-dialog',
  imports: [LucideAngularModule, Select, CommonModule, DatePickerModule, FormsModule],
  templateUrl: './add-task-dialog.html',
  styleUrl: './add-task-dialog.css'
})
export class AddTaskDialog implements OnInit {
  readonly List = List
  readonly Calendar = Calendar
  readonly Tag = Tag

  priorityOptions: SelectItem[] = [
    {
      value: "low",
      title: "Low"
    },
    {
      value: "medium",
      title: "Medium"
    },
    {
      value: "high",
      title: "High"
    }
  ]

  @Input() task?: TaskEntity
  @Input() column!: ColumnEntity
  columnList$: Observable<ColumnEntity[]>

  taskTitle = signal<string>('New Task')
  selectedPriority = signal<SelectItem | undefined>(undefined)
  selectedColumn = signal<SelectItem | undefined>(undefined)
  selectedDate = signal<Date | undefined>(undefined)
  date: Date | undefined
  taskDescription = signal<string>('')

  isLoaded = false;

  constructor(
    private store: Store,
  ) {
    this.columnList$ = this.store.select(selectSelectedBoardColumns)
  }

  ngOnInit(): void {
    if (this.column) {
      this.selectedColumn.set({
        value: this.column.id!,
        title: this.column.title!
      })
    }
    if (this.task) {
      this.taskTitle.set(this.task.title!)
      this.taskDescription.set(this.task.description!)
      this.selectedPriority.set(this.priorityOptions.find(po => po.value === this.task?.priority))
      this.selectedDate.set(this.task.dueDate)
    }
  }

  onTaskTitleChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const value: string = input.value;
    this.taskTitle.set(value)
  }

  onTaskDescriptionChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const value: string = input.value;
    this.taskDescription.set(value)
  }

  onPrioritySelect(item: SelectItem) {
    this.selectedPriority.set(item)
  }

  onColumnSelect(item: SelectItem) {
    this.selectedColumn.set(item)
  }

  columnsToSelectEntity(): Observable<SelectItem[]> {
    return this.columnList$.pipe(map(list => list.map(column => ({
      value: column.id!,
      title: column.title!
    }))))
  }

  getCurrentDate() {
    return new Date()
  }
}


