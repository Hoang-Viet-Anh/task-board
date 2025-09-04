import { Component, Input } from '@angular/core';
import { Separator } from "@app/shared/components/separator/separator";
import { EllipsisVertical, LucideAngularModule } from "lucide-angular";
import { ColumnMenu } from "./components/column-menu/column-menu";
import { AddTaskButton } from "./components/add-task-button/add-task-button";
import { TaskCard } from "./components/task-card/task-card";
import { ColumnEntity } from '../../models/column.model';
import { CdkDragDrop, DragDropModule } from '@angular/cdk/drag-drop';
import { TaskEntity } from '../../models/task.model';
import { changeTaskList } from '../../store/selected-board.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-task-list',
  imports: [Separator, LucideAngularModule, ColumnMenu, AddTaskButton, TaskCard, DragDropModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css'
})
export class TaskList {
  readonly EllipsisVertical = EllipsisVertical

  @Input() column!: ColumnEntity

  constructor(private store: Store) { }

  onDropped(event: CdkDragDrop<any>) {
    const task: TaskEntity = event.item.data;
    const oldColumn = event.previousContainer.data
    const newColumn = event.container.data

    if (oldColumn.id === newColumn.id)
      return;

    this.store.dispatch(changeTaskList({
      task,
      newColumn
    }))
  }
}
