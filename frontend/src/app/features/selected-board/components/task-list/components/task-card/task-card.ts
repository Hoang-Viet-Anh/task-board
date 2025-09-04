import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { Calendar, Dot, EllipsisVertical, LucideAngularModule } from "lucide-angular";
import { Badge } from "@app/shared/components/badge/badge";
import { ChangeListDropdown } from "./components/change-list-dropdown/change-list-dropdown";
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { priorityOptions } from '../add-task-button/components/add-task-dialog/components/task-priority-select/task-priority-select';
import { TaskMenu } from "./components/task-menu/task-menu";
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { DialogService } from '@app/shared/services/dialog.service';
import { TaskDialog } from './components/task-dialog/task-dialog';

@Component({
  selector: 'app-task-card',
  imports: [Card, CommonModule, LucideAngularModule, Badge, ChangeListDropdown, TaskMenu],
  templateUrl: './task-card.html',
  styleUrl: './task-card.css'
})
export class TaskCard {
  readonly EllipsisVertical = EllipsisVertical
  readonly Calendar = Calendar
  readonly Dot = Dot

  @Input() task!: TaskEntity
  @Input() column!: ColumnEntity

  constructor(private dialogService: DialogService) {

  }

  getPriorityTitle() {
    return priorityOptions.find(po => po.value === this.task.priority)?.title
  }

  openTaskDialog() {
    this.dialogService.open(TaskDialog, {
      task: this.task,
      column: this.column
    })
  }
}
