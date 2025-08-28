import { Component, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { Badge } from "@app/shared/components/badge/badge";
import { Card } from "@app/shared/components/card/card";
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { Drawer } from "@app/shared/components/drawer/drawer";
import { Dialog } from "@app/shared/components/dialog/dialog";
import { InputComponent } from "@app/shared/components/input/input";
import { EyeOff, LucideAngularModule } from "lucide-angular";
import { Select } from "@app/shared/components/select/select";
import { SelectItem } from '@app/shared/components/select/models/select-item.model';
import { Separator } from "@app/shared/components/separator/separator";

@Component({
  selector: 'app-login',
  imports: [Button, Badge, Card, DropdownMenu, Drawer, Dialog, InputComponent, LucideAngularModule, Select, Separator],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  readonly EyeOff = EyeOff

  dropdownOpen = signal<boolean>(false);
  drawerOpen = signal<boolean>(false);
  dialogOpen = signal<boolean>(false);
  inputValue = signal<string>('');
  selectValue = signal<SelectItem>({
    value: "todo",
    title: "To Do"
  });
  selectItems: SelectItem[] = [
    { value: 'todo', title: 'To Do' },
    { value: 'inprogress', title: 'In Progress' },
    { value: 'done', title: 'Done' },
    { value: 'archived', title: 'Archived' }
  ];

  onValueChange(value: string) {
    this.inputValue.set(value)
  }

  onDialogClick(state?: boolean) {
    this.dialogOpen.set(state ?? !this.dialogOpen())
  }

  onDropdownClick(state?: boolean) {
    this.dropdownOpen.set(state ?? !this.dropdownOpen())
  }

  onDrawerClick(state?: boolean) {
    this.drawerOpen.set(state ?? !this.drawerOpen())
  }

  onSelectedValue(item: SelectItem) {
    this.selectValue.set(item)
  }
}
