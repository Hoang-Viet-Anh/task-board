import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { LucideAngularModule, Plus } from "lucide-angular";
import { Button } from "@app/shared/components/button/button";
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { DialogService } from '@app/shared/services/dialog.service';
import { AddTaskDialog } from './components/add-task-dialog/add-task-dialog';

@Component({
  selector: 'app-add-task-button',
  imports: [CommonModule, LucideAngularModule, Button],
  templateUrl: './add-task-button.html',
  styleUrl: './add-task-button.css',
  host: {
    class: 'w-full'
  }
})
export class AddTaskButton {
  readonly Plus = Plus

  @Input() column!: ColumnEntity

  constructor(private dialogService: DialogService) { }

  onAddTaskDialogOpen() {
    this.dialogService.open(AddTaskDialog, {
      column: this.column
    })
  }
}
