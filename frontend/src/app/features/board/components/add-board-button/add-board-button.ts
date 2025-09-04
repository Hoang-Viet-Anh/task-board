import { Component, OnInit, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { LucideAngularModule, Plus } from "lucide-angular";
import { CommonModule } from '@angular/common';
import { Card } from "@app/shared/components/card/card";
import { Actions, ofType } from '@ngrx/effects';
import { createBoardSuccess, joinBoardSuccess } from './store/add-board.actions';
import { ReactiveFormsModule } from '@angular/forms';
import { DialogService } from '@app/shared/services/dialog.service';
import { AddBoardDialog } from './components/add-board-dialog/add-board-dialog';

@Component({
  selector: 'app-add-board-button',
  imports: [Button, CommonModule, LucideAngularModule, ReactiveFormsModule],
  templateUrl: './add-board-button.html',
  styleUrl: './add-board-button.css'
})
export class AddBoardButton {
  readonly Plus = Plus

  constructor(
    private actions$: Actions,
    private dialogService: DialogService
  ) {
    this.actions$.pipe(ofType(createBoardSuccess, joinBoardSuccess)).subscribe(() => {
      this.dialogService.close()
    })
  }

  openAddBoardDialog() {
    this.dialogService.open(AddBoardDialog)
  }

}
