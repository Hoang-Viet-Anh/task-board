import { Component, Input, OnInit } from '@angular/core';
import { Calendar, List, LucideAngularModule, Tag, UsersRound } from "lucide-angular";
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { DatePickerModule } from 'primeng/datepicker';
import { FormsModule } from '@angular/forms';
import { DialogLayout } from "@app/shared/modals/dialog-layout/dialog-layout";
import { DialogService } from '@app/shared/services/dialog.service';
import { priorityOptions } from '../../../add-task-button/components/add-task-dialog/components/task-priority-select/task-priority-select';
import { TaskLogs } from "./components/task-logs/task-logs";
import { TaskAssignee } from "./components/task-assignee/task-assignee";
import { Observable, of } from 'rxjs';
import { selectColumnById, selectTaskById } from '@app/features/selected-board/store/selected-board.selectors';

@Component({
  selector: 'app-task-dialog',
  imports: [LucideAngularModule, CommonModule, DatePickerModule, FormsModule, DialogLayout, TaskLogs, TaskAssignee],
  templateUrl: './task-dialog.html',
  styleUrl: './task-dialog.css'
})
export class TaskDialog implements OnInit {
  readonly List = List
  readonly Calendar = Calendar
  readonly Tag = Tag
  readonly UsersRound = UsersRound

  @Input() taskId: string = '';
  @Input() columnId: string = '';

  task$: Observable<TaskEntity | undefined> = of()
  column$: Observable<ColumnEntity | undefined> = of()

  constructor(
    private store: Store,
    private dialogService: DialogService
  ) { }

  ngOnInit(): void {
    this.task$ = this.store.select(selectTaskById(this.taskId))
    this.column$ = this.store.select(selectColumnById(this.columnId))
  }

  closeDialog() {
    this.dialogService.close()
  }

  getPriority(task: TaskEntity) {
    return priorityOptions.find(po => po.value === task.priority)?.title
  }

}
