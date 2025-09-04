import { CommonModule } from '@angular/common';
import { Component, Input, signal } from '@angular/core';
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { Button } from "@app/shared/components/button/button";
import { ChevronDown, LucideAngularModule } from "lucide-angular";
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { selectSelectedBoardColumns } from '@app/features/selected-board/store/selected-board.selectors';
import { changeTaskList } from '@app/features/selected-board/store/selected-board.actions';

@Component({
  selector: 'app-change-list-dropdown',
  imports: [DropdownMenu, CommonModule, Button, LucideAngularModule],
  templateUrl: './change-list-dropdown.html',
  styleUrl: './change-list-dropdown.css'
})
export class ChangeListDropdown {
  readonly ChevronDown = ChevronDown

  @Input() task!: TaskEntity;
  @Input() column!: ColumnEntity;

  columnList$: Observable<ColumnEntity[]>;

  open = signal<boolean>(false)

  constructor(
    private store: Store
  ) {
    this.columnList$ = this.store.select(selectSelectedBoardColumns)
  }

  toggleDropdown(state?: boolean, event?: Event) {
    if (event) event.stopPropagation()
    this.open.set(state ?? !this.open());
  }

  onChangeColumn(selectedColumn: ColumnEntity) {
    if (selectedColumn.id == this.column.id)
      return;

    this.store.dispatch(changeTaskList({ task: this.task, currentColumn: selectedColumn, previousColumn: this.column }))
  }
}
