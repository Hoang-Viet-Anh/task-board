import { Component, Input, signal } from '@angular/core';
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { UserEntity } from '@app/features/selected-board/models/user.model';
import { selectBoardMembers, selectSelectedBoardColumns } from '@app/features/selected-board/store/selected-board.selectors';
import { Store } from '@ngrx/store';
import { map, Observable, of } from 'rxjs';
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { Button } from "@app/shared/components/button/button";
import { Check, ChevronDown, LucideAngularModule } from "lucide-angular";
import { CommonModule } from '@angular/common';
import { assignTaskRequest } from '@app/features/selected-board/components/task-list/store/task.actions';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';

@Component({
  selector: 'app-task-assignee',
  imports: [DropdownMenu, Button, LucideAngularModule, CommonModule],
  templateUrl: './task-assignee.html',
  styleUrl: './task-assignee.css'
})
export class TaskAssignee {
  readonly Check = Check
  readonly ChevronDown = ChevronDown

  @Input() task!: TaskEntity
  @Input() column!: ColumnEntity

  boardMembers$: Observable<UserEntity[] | undefined>

  isDropdownOpen = signal<boolean>(false)

  constructor(private store: Store) {
    this.boardMembers$ = this.store.select(selectBoardMembers)
  }

  toggleDropdown(state?: boolean) {
    this.isDropdownOpen.set(state ?? !this.isDropdownOpen())
  }

  preventClose(event: Event) {
    event.stopPropagation()
  }

  isAssigned(user: UserEntity): boolean {
    return !!this.task.assignedUsers?.find(au => au.id === user.id)
  }

  onAssign(user: UserEntity) {
    let updatedAssignedList = [...this.task.assignedUsers ?? []]
    if (this.isAssigned(user))
      updatedAssignedList.push(user)
    else
      updatedAssignedList = updatedAssignedList.filter(au => au.id !== user.id)

    const updatedTask: TaskEntity = {
      ...this.task,
      assignedUsers: updatedAssignedList
    }

    this.store.dispatch(assignTaskRequest({ task: updatedTask, userId: user.id!, boardId: this.column.boardId! }))
  }
}
