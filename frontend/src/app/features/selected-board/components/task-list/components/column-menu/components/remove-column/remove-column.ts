import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { LoaderCircle, LucideAngularModule } from "lucide-angular";
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { selectRemoveColumnStatus } from '../../store/column-menu.selectors';
import { removeColumnRequest } from '../../store/column-menu.actions';

@Component({
  selector: 'app-remove-column',
  imports: [Button, LucideAngularModule, CommonModule],
  templateUrl: './remove-column.html',
  styleUrl: './remove-column.css'
})
export class RemoveColumn {
  readonly LoaderCircle = LoaderCircle

  isLoading$: Observable<boolean>
  isSuccess$: Observable<boolean>

  @Input() column!: ColumnEntity
  @Output() onClose = new EventEmitter<void>()

  constructor(private readonly store: Store) {
    this.isLoading$ = this.store.select(selectRemoveColumnStatus).pipe(map(status => status.isLoading))
    this.isSuccess$ = this.store.select(selectRemoveColumnStatus).pipe(map(status => status.isSuccess))

    this.isSuccess$.subscribe(state => {
      if (state) this.closeDialog()
    })
  }

  closeDialog() {
    this.onClose.emit()
  }

  onRemoveList() {
    this.store.dispatch(removeColumnRequest(this.column))
  }
}
