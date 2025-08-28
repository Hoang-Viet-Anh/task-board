import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Button } from "../button/button";
import { LucideAngularModule, X } from "lucide-angular";

@Component({
  selector: 'app-drawer',
  imports: [CommonModule, Button, LucideAngularModule],
  templateUrl: './drawer.html',
  styleUrl: './drawer.css'
})
export class Drawer {
  readonly X = X;

  @Input() open: boolean = false;
  @Output() onClose = new EventEmitter<void>();

  closeDrawer() {
    this.onClose.emit();
  }

  drawerClick(event: Event) {
    event.stopPropagation();
  }
}
