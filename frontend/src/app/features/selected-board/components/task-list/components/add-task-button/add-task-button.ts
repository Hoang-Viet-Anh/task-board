import { CommonModule } from '@angular/common';
import { Component, Input, signal } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { LucideAngularModule, Plus } from "lucide-angular";
import { Button } from "@app/shared/components/button/button";
import { Dialog } from "@app/shared/components/dialog/dialog";
import { AddTaskDialog } from "./components/add-task-dialog/add-task-dialog";
import { ColumnEntity } from '@app/features/selected-board/models/column.model';

@Component({
  selector: 'app-add-task-button',
  imports: [Card, CommonModule, LucideAngularModule, Button, Dialog, AddTaskDialog],
  templateUrl: './add-task-button.html',
  styleUrl: './add-task-button.css',
  host: {
    class: 'w-full'
  }
})
export class AddTaskButton {
  readonly Plus = Plus

  @Input() column!: ColumnEntity

  openDialog = signal<boolean>(false)

  toggleDialog(state?: boolean) {
    this.openDialog.set(state ?? !this.openDialog())
  }

}
