import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { selectRemoveBoardStatus } from '@app/features/board/store/board.selectors';
import { Button } from "@app/shared/components/button/button";
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { LoaderCircle, LucideAngularModule } from "lucide-angular";
import { BoardEntity } from '@app/features/board/models/board.model';
import { removeBoard } from '@app/features/board/store/board.actions';

@Component({
  selector: 'app-remove-board',
  imports: [Button, CommonModule, LucideAngularModule],
  templateUrl: './remove-board.html',
  styleUrl: './remove-board.css'
})
export class RemoveBoard {
  readonly LoaderCircle = LoaderCircle
  isLoading$: Observable<boolean>
  @Input() board?: BoardEntity

  @Output() onClose = new EventEmitter<void>()

  constructor(private readonly store: Store) {
    this.isLoading$ = this.store.select(selectRemoveBoardStatus).pipe(map(state => state?.isLoading))
  }

  closeDialog() {
    this.onClose.emit()
  }

  onRemoveBoard() {
    this.store.dispatch(removeBoard({ id: this.board?.id! }))
  }
}
