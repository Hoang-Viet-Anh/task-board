import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TaskEntity } from '@app/features/selected-board/models/task.model';
import { Dot, LucideAngularModule } from "lucide-angular";
import { toHTML } from 'slack-markdown';

@Component({
  selector: 'app-task-logs',
  imports: [CommonModule, LucideAngularModule],
  templateUrl: './task-logs.html',
  styleUrl: './task-logs.css',
})
export class TaskLogs {
  readonly Dot = Dot

  @Input() task!: TaskEntity

  parseSlackMarkdown(log: string) {
    return toHTML(log)
  }
}
