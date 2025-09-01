import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { EllipsisVertical, LucideAngularModule } from "lucide-angular";
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";

@Component({
  selector: 'app-column-menu',
  imports: [Button, LucideAngularModule, CommonModule, DropdownMenu],
  templateUrl: './column-menu.html',
  styleUrl: './column-menu.css'
})
export class ColumnMenu {
  readonly EllipsisVertical = EllipsisVertical

  open = signal<boolean>(false)

  toggleDropdown(state?: boolean) {
    this.open.set(state ?? !this.open())
  }
}
