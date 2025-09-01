import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { Button } from "@app/shared/components/button/button";
import { ChevronDown, LucideAngularModule } from "lucide-angular";

@Component({
  selector: 'app-change-list-dropdown',
  imports: [DropdownMenu, CommonModule, Button, LucideAngularModule],
  templateUrl: './change-list-dropdown.html',
  styleUrl: './change-list-dropdown.css'
})
export class ChangeListDropdown {
  readonly ChevronDown = ChevronDown

  open = signal<boolean>(false)

  toggleDropdown(state?: boolean) {
    this.open.set(state ?? !this.open());
  }
}
