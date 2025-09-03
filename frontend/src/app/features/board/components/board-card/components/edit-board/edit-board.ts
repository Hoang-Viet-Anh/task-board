import { Component, Input } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { BoardEntity } from '@app/features/board/models/board.model';
import { DialogService } from '@app/shared/services/dialog.service';
import { LucideAngularModule, Pencil } from 'lucide-angular';
import { EditBoardDialog } from './components/edit-board-dialog/edit-board-dialog';

@Component({
  selector: 'app-edit-board',
  imports: [LucideAngularModule, Button],
  templateUrl: './edit-board.html',
  styleUrl: './edit-board.css'
})
export class EditBoard {
  readonly Pencil = Pencil

  @Input() board!: BoardEntity

  constructor(private dialogService: DialogService) { }

  openEditDialog() {
    this.dialogService.open(EditBoardDialog, {
      board: this.board
    })
  }
}
