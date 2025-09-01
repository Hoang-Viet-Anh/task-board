import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  imports: [CommonModule],
  templateUrl: './button.html',
  styleUrl: './button.css'
})
export class Button {
  @Input() variant: 'primary' | 'secondary' | 'outline' | 'ghost' | 'link' | 'destruction' = 'primary';
  @Input() disabled: boolean = false;
  @Input() class: string = '';

  @Output() clicked = new EventEmitter<void>();

  onClick() {
    this.clicked.emit();
  }
}
