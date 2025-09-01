import { Component, Input, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-edit-board',
  imports: [LucideAngularModule, Button, InputComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './edit-board.html',
  styleUrl: './edit-board.css'
})
export class EditBoard implements OnInit {
  @Input() board?: BoardEntity
  readonly LoaderCircle = LoaderCircle

  editBoardForm: FormGroup = new FormGroup({
    boardTitle: new FormControl('', Validators.required)
  })

  isLoading$: Observable<boolean>;

  constructor(private readonly store: Store) {
    this.isLoading$ = this.store.select(selectUpdateBoardStatus).pipe(map(status => status?.isLoading))
  }
  ngOnInit(): void {
    if (this.board) {
      this.editBoardForm.patchValue({
        boardTitle: this.board?.title
      })
    }
  }

  onEditSubmit() {
    if (this.editBoardForm.valid) {
      this.store.dispatch(updateBoard({
        ...this.board!,
        title: this.editBoardForm.value.boardTitle
      }));
    } else {
      this.editBoardForm.markAsTouched()
    }
  }

  onBoardTitleChange(value: string) {
    this.editBoardForm.patchValue({ boardTitle: value })
  }
}
