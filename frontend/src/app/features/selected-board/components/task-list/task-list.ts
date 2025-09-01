import { Component } from '@angular/core';
import { Separator } from "@app/shared/components/separator/separator";
import { EllipsisVertical, LucideAngularModule } from "lucide-angular";
import { ColumnMenu } from "./components/column-menu/column-menu";
import { AddTaskButton } from "./components/add-task-button/add-task-button";
import { TaskCard } from "./components/task-card/task-card";

@Component({
  selector: 'app-task-list',
  imports: [Separator, LucideAngularModule, ColumnMenu, AddTaskButton, TaskCard],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css'
})
export class TaskList {
  readonly EllipsisVertical = EllipsisVertical

}
