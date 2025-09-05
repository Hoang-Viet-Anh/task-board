import { Component, Input, OnInit, signal } from '@angular/core';
import { Separator } from "@app/shared/components/separator/separator";
import { EllipsisVertical, LucideAngularModule } from "lucide-angular";
import { ColumnMenu } from "./components/column-menu/column-menu";
import { AddTaskButton } from "./components/add-task-button/add-task-button";
import { TaskCard } from "./components/task-card/task-card";
import { ColumnEntity } from '../../models/column.model';
import { CdkDragDrop, DragDropModule, transferArrayItem } from '@angular/cdk/drag-drop';
import { TaskEntity } from '../../models/task.model';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { changeTaskList } from './store/task.actions';

@Component({
  selector: 'app-task-list',
  imports: [Separator, LucideAngularModule, ColumnMenu, AddTaskButton, TaskCard, DragDropModule, CommonModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskList implements OnInit {
  readonly EllipsisVertical = EllipsisVertical

  @Input() column!: ColumnEntity
  taskArray: TaskEntity[] = []

  isDragged = signal<boolean>(false)

  constructor(private store: Store) { }

  ngOnInit(): void {
    this.taskArray = this.column.tasks ? [...this.column.tasks] : []
  }

  onDragStart() {
    this.isDragged.set(true)
  }

  onDropped(event: CdkDragDrop<any>) {
    this.isDragged.set(false)

    const previousColumn: ColumnEntity = event.previousContainer.data
    const currentColumn: ColumnEntity = event.container.data
    const previousIndex = event.previousIndex
    const currentIndex = event.currentIndex

    if (previousColumn.id === currentColumn.id)
      return;

    let previousList = [...previousColumn.tasks ?? []]
    let currentList = [...currentColumn.tasks ?? []]

    const task = previousList[previousIndex]

    transferArrayItem(previousList, currentList, previousIndex, currentIndex)

    const newCurrentColumn: ColumnEntity = {
      ...currentColumn,
      tasks: [...currentList]
    }
    const newPreviousColumn: ColumnEntity = {
      ...previousColumn,
      tasks: [...previousList]
    }

    this.store.dispatch(changeTaskList({
      task,
      currentColumn: newCurrentColumn,
      previousColumn: newPreviousColumn
    }))
  }
}
