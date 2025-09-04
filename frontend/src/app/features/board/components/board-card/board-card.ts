import { Component, Input, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { BoardEntity } from '../../models/board.model';
import { Router } from '@angular/router';
import { EllipsisVertical, LucideAngularModule, Pencil, Trash } from "lucide-angular";
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { RemoveBoard } from "./components/remove-board/remove-board";
import { EditBoard } from "./components/edit-board/edit-board";
import { Store } from '@ngrx/store';
import { clearBoard } from '@app/features/selected-board/store/selected-board.actions';

@Component({
  selector: 'app-board-card',
  imports: [Button, LucideAngularModule, DropdownMenu, RemoveBoard, EditBoard],
  templateUrl: './board-card.html',
  styleUrl: './board-card.css'
})
export class BoardCard {
  readonly EllipsisVertical = EllipsisVertical
  readonly Pencil = Pencil
  readonly Trash = Trash

  @Input() board!: BoardEntity

  openDropdown = signal<boolean>(false)

  constructor(
    private readonly router: Router,
    private readonly store: Store,
  ) { }

  navigateToBoard(boardId: string) {
    this.store.dispatch(clearBoard())
    this.router.navigate(['/board', boardId]);
  }

  toggleDropdown(state?: boolean) {
    this.openDropdown.set(state ?? !this.openDropdown())
  }

}
