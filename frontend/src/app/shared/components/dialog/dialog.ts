import { CommonModule, } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Card } from "../card/card";
import { Button } from "../button/button";
import { LucideAngularModule, X } from 'lucide-angular';

@Component({
  selector: 'app-dialog',
  imports: [CommonModule, Card, Button, LucideAngularModule],
  templateUrl: './dialog.html',
  styleUrl: './dialog.css'
})
export class Dialog {
  readonly X = X;

  @Input() open: boolean = false;
  @Input() cardClass: string = '';
  @Input() closeButtonClass: string = '';
  @Output() onClose = new EventEmitter<void>();

  closeDialog() {
    this.onClose.emit()
  }

  onDialogClick(event: Event) {
    event.stopPropagation();
  }
}
