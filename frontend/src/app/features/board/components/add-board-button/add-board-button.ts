import { Component, OnInit, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { LucideAngularModule, Plus } from "lucide-angular";
import { CommonModule } from '@angular/common';
import { Dialog } from '@app/shared/components/dialog/dialog';
import { Card } from "@app/shared/components/card/card";
import { Separator } from "@app/shared/components/separator/separator";
import { Actions, ofType } from '@ngrx/effects';
import { createBoardSuccess, joinBoardSuccess } from './store/add-board.actions';
import { ReactiveFormsModule } from '@angular/forms';
import { CreateBoard } from "./components/create-board/create-board";
import { JoinBoard } from "./components/join-board/join-board";
import { NavigationStart, Router } from '@angular/router';

@Component({
  selector: 'app-add-board-button',
  imports: [Dialog, Button, CommonModule, LucideAngularModule, Card, Separator, ReactiveFormsModule, CreateBoard, JoinBoard],
  templateUrl: './add-board-button.html',
  styleUrl: './add-board-button.css'
})
export class AddBoardButton {
  readonly Plus = Plus

  addBoardDialogOpen = signal<boolean>(false)

  constructor(
    private actions$: Actions,
  ) {
    this.actions$.pipe(ofType(createBoardSuccess, joinBoardSuccess)).subscribe(() => {
      this.toggleBoardDialog(false)
    })
  }

  toggleBoardDialog(state?: boolean) {
    this.addBoardDialogOpen.set(state ?? !this.addBoardDialogOpen())
  }

}
