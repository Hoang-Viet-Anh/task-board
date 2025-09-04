import { Component, Input, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { LoaderCircle, LucideAngularModule } from "lucide-angular";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { BoardEntity } from '@app/features/board/models/board.model';
import { selectUpdateBoardStatus } from '@app/features/board/store/board.selectors';
import { updateBoard } from '@app/features/board/store/board.actions';
import { InputComponent } from '@app/shared/components/input/input';
import { CommonModule } from '@angular/common';
import { DialogLayout } from '@app/shared/modals/dialog-layout/dialog-layout';
import { DialogService } from '@app/shared/services/dialog.service';

@Component({
  selector: 'app-edit-board-dialog',
  imports: [DialogLayout, LucideAngularModule, Button, InputComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './edit-board-dialog.html',
  styleUrl: './edit-board-dialog.css'
})
export class EditBoardDialog {
  readonly LoaderCircle = LoaderCircle

  @Input() board!: BoardEntity

  boardTitle = signal<string>('')
  isLoading$: Observable<boolean>;

  constructor(
    private store: Store,
    private dialogService: DialogService
  ) {
    this.isLoading$ = this.store.select(selectUpdateBoardStatus).pipe(map(status => status?.isLoading))
  }

  ngOnInit(): void {
    if (this.board) {
      this.boardTitle.set(this.board.title)
    }
  }

  onEditSubmit() {
    if (this.boardTitle().length === 0 || this.boardTitle() === this.board.title) {
      this.dialogService.close()
    } else {
      this.store.dispatch(updateBoard({
        id: this.board.id!,
        title: this.boardTitle()
      }));
    }
  }

  onBoardTitleChange(value: string) {
    this.boardTitle.set(value)
  }
}
