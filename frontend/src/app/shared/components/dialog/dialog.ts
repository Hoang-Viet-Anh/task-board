import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { Card } from "../card/card";
import { Button } from "../button/button";
import { LucideAngularModule, X } from 'lucide-angular';

@Component({
  selector: 'app-dialog',
  imports: [CommonModule, Card, Button, LucideAngularModule],
  templateUrl: './dialog.html',
  styleUrl: './dialog.css'
})
export class Dialog implements AfterViewInit {
  readonly X = X;

  @Input() open: boolean = false;
  @Output() onClose = new EventEmitter<void>();

  isLoaded = signal(false)

  ngAfterViewInit(): void {
    requestAnimationFrame(() => this.isLoaded.set(true))
  }

  closeDialog() {
    this.onClose.emit()
  }

  onDialogClick(event: Event) {
    event.stopPropagation();
  }
}
