import { Component, Input, signal } from '@angular/core';
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { Button } from "@app/shared/components/button/button";
import { EllipsisVertical, LucideAngularModule, Pencil, Trash } from "lucide-angular"; import { Store } from '@ngrx/store';
import { DialogService } from '@app/shared/services/dialog.service';
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { AddTaskDialog } from '../../../add-task-button/components/add-task-dialog/add-task-dialog';
import { deleteTaskRequest } from '../../../../store/task.actions';
import { DeleteDialog } from '@app/shared/modals/delete-dialog/delete-dialog';
;

@Component({
  selector: 'app-task-menu',
  imports: [DropdownMenu, Button, LucideAngularModule],
  templateUrl: './task-menu.html',
  styleUrl: './task-menu.css'
})
export class TaskMenu {
  readonly EllipsisVertical = EllipsisVertical
  readonly Pencil = Pencil
  readonly Trash = Trash

  @Input() task!: TaskEntity;
  @Input() column!: ColumnEntity;

  isDropdownOpen = signal<boolean>(false)

  constructor(
    private store: Store,
    private dialogService: DialogService
  ) { }

  toggleDropdown(state?: boolean) {
    this.isDropdownOpen.set(state ?? !this.isDropdownOpen())
  }

  openEditDialog() {
    this.dialogService.open(AddTaskDialog, {
      task: this.task,
      column: this.column
    })
  }

  openRemoveDialog() {
    this.dialogService.open(DeleteDialog, {
      title: "Remove Task",
      description: "Are you sure you want to delete this task? This action cannot be undone.",
      onDelete: () => this.onTaskRemove()
    })
  }

  onTaskRemove() {
    this.store.dispatch(deleteTaskRequest({ taskId: this.task.id!, boardId: this.column.boardId! }))
  }
}
