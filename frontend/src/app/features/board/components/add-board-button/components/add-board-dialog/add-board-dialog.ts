import { Component } from '@angular/core';
import { DialogLayout } from "@app/shared/modals/dialog-layout/dialog-layout";
import { CreateBoard } from "../create-board/create-board";
import { Separator } from "@app/shared/components/separator/separator";
import { JoinBoard } from "../join-board/join-board";

@Component({
  selector: 'app-add-board-dialog',
  imports: [DialogLayout, CreateBoard, Separator, JoinBoard],
  templateUrl: './add-board-dialog.html',
  styleUrl: './add-board-dialog.css'
})
export class AddBoardDialog {

}
