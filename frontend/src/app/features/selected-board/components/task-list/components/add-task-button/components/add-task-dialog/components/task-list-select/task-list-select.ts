import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { selectSelectedBoardColumns } from '@app/features/selected-board/store/selected-board.selectors';
import { SelectItem } from '@app/shared/components/select/models/select-item.model';
import { Select } from "@app/shared/components/select/select";
import { Store } from '@ngrx/store';
import { List, LucideAngularModule } from "lucide-angular";
import { map, Observable } from 'rxjs';

@Component({
  selector: 'app-task-list-select',
  imports: [Select, LucideAngularModule, CommonModule],
  templateUrl: './task-list-select.html',
  styleUrl: './task-list-select.css'
})
export class TaskListSelect {
  readonly List = List

  columnList$: Observable<ColumnEntity[]>;

  @Input() value: SelectItem | undefined
  @Output() onSelect = new EventEmitter<SelectItem>()

  constructor(
    private store: Store,
  ) {
    this.columnList$ = this.store.select(selectSelectedBoardColumns)
  }

  onListSelect(value: SelectItem) {
    this.onSelect.emit(value)
  }

  columnsToSelectEntity(): Observable<SelectItem[]> {
    return this.columnList$.pipe(map(list => list.map(column => ({
      value: column.id!,
      title: column.title!
    }))))
  }
}
