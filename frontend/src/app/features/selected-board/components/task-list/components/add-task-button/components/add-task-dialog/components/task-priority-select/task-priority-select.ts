import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LucideAngularModule, Tag } from "lucide-angular";
import { Select } from "@app/shared/components/select/select";
import { SelectItem } from '@app/shared/components/select/models/select-item.model';

@Component({
  selector: 'app-task-priority-select',
  imports: [LucideAngularModule, Select],
  templateUrl: './task-priority-select.html',
  styleUrl: './task-priority-select.css'
})
export class TaskPrioritySelect {
  readonly Tag = Tag
  readonly priorityOptions = priorityOptions

  @Input() value: SelectItem | undefined
  @Output() onSelect = new EventEmitter<SelectItem>()

  onPrioritySelect(value: SelectItem) {
    this.onSelect.emit(value)
  }

}

export const priorityOptions: SelectItem[] = [
  {
    value: "low",
    title: "Low"
  },
  {
    value: "medium",
    title: "Medium"
  },
  {
    value: "high",
    title: "High"
  }
]