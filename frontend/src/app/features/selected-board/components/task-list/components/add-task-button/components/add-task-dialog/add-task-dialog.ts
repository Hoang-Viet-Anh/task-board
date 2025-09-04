import { Component, Input, OnInit, signal } from '@angular/core';
import { Calendar, List, LoaderCircle, LucideAngularModule } from "lucide-angular";
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { SelectItem } from '@app/shared/components/select/models/select-item.model';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { DatePickerModule } from 'primeng/datepicker';
import { FormsModule } from '@angular/forms';
import { DialogLayout } from "@app/shared/modals/dialog-layout/dialog-layout";
import { TaskDescriptionInput } from "./components/task-description-input/task-description-input";
import { priorityOptions, TaskPrioritySelect } from "./components/task-priority-select/task-priority-select";
import { TaskListSelect } from "./components/task-list-select/task-list-select";
import { TaskDateSelect } from "./components/task-date-select/task-date-select";
import { Button } from "@app/shared/components/button/button";
import { DialogService } from '@app/shared/services/dialog.service';
import { createTaskRequest, updateTaskRequest } from '../../../../store/task.actions';
import { map, Observable } from 'rxjs';
import { selectCreateTaskStatus, selectUpdateTaskStatus } from '../../../../store/task.selectors';

@Component({
  selector: 'app-add-task-dialog',
  imports: [LucideAngularModule, CommonModule, DatePickerModule, FormsModule, DialogLayout, TaskDescriptionInput, TaskPrioritySelect, TaskListSelect, TaskDateSelect, Button],
  templateUrl: './add-task-dialog.html',
  styleUrl: './add-task-dialog.css'
})
export class AddTaskDialog implements OnInit {
  readonly LoaderCircle = LoaderCircle

  @Input() task?: TaskEntity
  @Input() column!: ColumnEntity

  taskTitle = signal<string>('New Task')
  taskDescription = signal<string>('')

  selectedList = signal<SelectItem | undefined>(undefined)
  selectedDate = signal<Date>(new Date())
  selectedPriority = signal<SelectItem | undefined>(priorityOptions[1])

  isCreateLoading$: Observable<boolean>
  isUpdateLoading$: Observable<boolean>

  constructor(
    private store: Store,
    private dialogService: DialogService
  ) {
    this.isCreateLoading$ = this.store.select(selectCreateTaskStatus).pipe(map(state => state.isLoading))
    this.isUpdateLoading$ = this.store.select(selectUpdateTaskStatus).pipe(map(state => state.isLoading))
  }

  ngOnInit(): void {
    if (this.column) {
      this.selectedList.set({
        value: this.column.id!,
        title: this.column.title!
      })
    }
    if (this.task) {
      this.taskTitle.set(this.task.title!)
      this.taskDescription.set(this.task.description!)
      this.selectedDate.set(new Date(this.task.dueDate!))
      this.selectedPriority.set(priorityOptions.find(po => po.value === this.task?.priority))
    }
  }

  closeDialog() {
    this.dialogService.close()
  }

  onSave() {
    const newTask: TaskEntity = {
      id: this.task?.id,
      title: this.taskTitle(),
      description: this.taskDescription(),
      columnId: this.selectedList()?.value,
      dueDate: this.selectedDate(),
      priority: this.selectedPriority()?.value
    }

    newTask.dueDate?.setHours(0, 0, 0, 0)

    if (newTask.id === undefined)
      this.store.dispatch(createTaskRequest({
        task: newTask,
        boardId: this.column.boardId!
      }))
    else
      this.store.dispatch(updateTaskRequest({
        task: newTask,
        boardId: this.column.boardId!
      }))
  }

  onTaskTitleChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const value: string = input.value;
    this.taskTitle.set(value)
  }

  onTaskDescriptionChange(value: string) {
    this.taskDescription.set(value)
  }

  onListSelect(item: SelectItem) {
    this.selectedList.set(item)
  }

  onDateSelect(newDate: Date) {
    this.selectedDate.set(newDate)
  }

  onPrioritySelect(item: SelectItem) {
    this.selectedPriority.set(item)
  }

  getIsLoading(): Observable<boolean> {
    return this.isCreateLoading$ || this.isUpdateLoading$
  }
}


