import { Component, Input, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { Card } from "@app/shared/components/card/card";
import { BoardEntity } from '../../models/board.model';
import { Router } from '@angular/router';
import { EllipsisVertical, LucideAngularModule, Pencil, Trash } from "lucide-angular";
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { Dialog } from "@app/shared/components/dialog/dialog";
import { RemoveBoard } from "./components/remove-board/remove-board";
import { EditBoard } from "./components/edit-board/edit-board";
import { combineLatest, map, Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { selectRemoveBoardStatus, selectUpdateBoardStatus } from '../../store/board.selectors';
import { Actions, ofType } from '@ngrx/effects';
import { removeBoardSuccess, updateBoardSuccess } from '../../store/board.actions';
import { clearBoard } from '@app/features/selected-board/store/selected-board.actions';

@Component({
  selector: 'app-board-card',
  imports: [Button, Card, LucideAngularModule, DropdownMenu, Dialog, RemoveBoard, EditBoard],
  templateUrl: './board-card.html',
  styleUrl: './board-card.css'
})
export class BoardCard {
  readonly EllipsisVertical = EllipsisVertical
  readonly Pencil = Pencil
  readonly Trash = Trash

  @Input() board!: BoardEntity

  isRemoveLoading$: Observable<boolean>;
  isUpdateLoading$: Observable<boolean>;

  openDropdown = signal<boolean>(false)
  openDialog = signal<boolean>(false)
  isEditContent = signal<boolean>(false)
  isLoading = signal<boolean>(false)

  constructor(
    private readonly router: Router,
    private readonly store: Store,
    private actions$: Actions
  ) {
    this.actions$.pipe(ofType(updateBoardSuccess, removeBoardSuccess)).subscribe(() => { this.toggleDialog(false) })

    this.isRemoveLoading$ = this.store.select(selectRemoveBoardStatus).pipe(map(status => status.isLoading))
    this.isUpdateLoading$ = this.store.select(selectUpdateBoardStatus).pipe(map(status => status.isLoading))

    combineLatest([
      this.isRemoveLoading$,
      this.isUpdateLoading$
    ]).subscribe(([removeStatus, updateStatus]) => {
      if (!removeStatus && !updateStatus) {
        this.isLoading.set(false)
        return;
      }
      this.isLoading.set(true)
    })
  }

  navigateToBoard(boardId: string) {
    this.store.dispatch(clearBoard())
    this.router.navigate(['/board', boardId]);
  }

  toggleDropdown(state?: boolean) {
    this.openDropdown.set(state ?? !this.openDropdown())
  }

  toggleDialog(state?: boolean, isEditContent?: boolean) {
    if (this.isLoading()) return;
    this.toggleDropdown(false)
    this.isEditContent.set(isEditContent ?? this.isEditContent())
    this.openDialog.set(state ?? !this.openDialog())
  }
}
