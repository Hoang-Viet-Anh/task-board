import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-task-description-input',
  imports: [],
  templateUrl: './task-description-input.html',
  styleUrl: './task-description-input.css'
})
export class TaskDescriptionInput {
  @Input() value: string = ''
  @Output() onInput = new EventEmitter<string>()

  onChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const value: string = input.value;
    this.onInput.emit(value)
  }
}
