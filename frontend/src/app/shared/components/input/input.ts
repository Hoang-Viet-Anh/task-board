import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { Card } from "../card/card";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-input',
  imports: [Card, CommonModule],
  templateUrl: './input.html',
  styleUrl: './input.css'
})
export class InputComponent {
  @Input() type: string = 'text';
  @Input() disabled: boolean = false;
  @Input() value: string = '';
  @Input() id: string = '';
  @Input() name: string = '';
  @Input() placeholder: string = '';

  @Output() onValueChange = new EventEmitter<string>();
  @ViewChild('inputElement') inputElement!: ElementRef<HTMLInputElement>;

  onChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const value: string = input.value;
    this.onValueChange.emit(value);
  }

  focusInput() {
    this.inputElement.nativeElement.focus();
  }
}
