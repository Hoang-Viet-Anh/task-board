import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { selectRemoveBoardStatus } from '@app/features/board/store/board.selectors';
import { Button } from "@app/shared/components/button/button";
import { Store } from '@ngrx/store';
import { firstValueFrom, map, Observable } from 'rxjs';
import { LoaderCircle, LucideAngularModule, Trash } from "lucide-angular";
import { BoardEntity } from '@app/features/board/models/board.model';
import { removeBoard } from '@app/features/board/store/board.actions';
import { DialogService } from '@app/shared/services/dialog.service';
import { DeleteDialog } from '@app/shared/modals/delete-dialog/delete-dialog';

@Component({
  selector: 'app-remove-board',
  imports: [Button, CommonModule, LucideAngularModule],
  templateUrl: './remove-board.html',
  styleUrl: './remove-board.css'
})
export class RemoveBoard {
  readonly Trash = Trash
  readonly LoaderCircle = LoaderCircle

  isLoading$: Observable<boolean>

  @Input() board?: BoardEntity

  constructor(
    private store: Store,
    private dialogService: DialogService
  ) {
    this.isLoading$ = this.store.select(selectRemoveBoardStatus).pipe(map(state => state?.isLoading))
  }

  async openDialog() {
    this.dialogService.open(DeleteDialog, {
      title: 'Remove Board',
      description: 'Are you sure you want to delete this board? This action cannot be undone.',
      isLoading: await firstValueFrom(this.isLoading$),
      onDelete: () => this.onDeleteBoard()
    })
  }

  onDeleteBoard() {
    this.store.dispatch(removeBoard(this.board!))
  }
}
