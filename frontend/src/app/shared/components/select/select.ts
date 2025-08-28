import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { DropdownMenu } from "../dropdown-menu/dropdown-menu";
import { CommonModule } from '@angular/common';
import { Button } from "../button/button";
import { SelectItem } from './models/select-item.model';
import { ChevronDown, LucideAngularModule } from "lucide-angular";

@Component({
  selector: 'app-select',
  imports: [DropdownMenu, CommonModule, Button, LucideAngularModule],
  templateUrl: './select.html',
  styleUrl: './select.css'
})
export class Select {
  readonly ChevronDown = ChevronDown;

  @Input() title: string = 'Select item';
  @Input() value?: SelectItem;
  @Input() items: SelectItem[] = [];
  @Output() onSelected = new EventEmitter<SelectItem>();

  @Input() class: string = '';

  open = signal<boolean>(false);

  toggleSelect(state?: boolean) {
    this.open.set(state ?? !this.open())
  }

  selectItem(item: SelectItem) {
    this.onSelected.emit(item);
    this.toggleSelect(false);
  }
}
