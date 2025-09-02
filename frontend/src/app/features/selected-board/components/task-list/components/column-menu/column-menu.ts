import { CommonModule } from '@angular/common';
import { Component, Input, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { EllipsisVertical, LucideAngularModule, Pencil, Trash } from "lucide-angular";
import { DropdownMenu } from "@app/shared/components/dropdown-menu/dropdown-menu";
import { Dialog } from "@app/shared/components/dialog/dialog";
import { RemoveColumn } from "./components/remove-column/remove-column";
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { ListTitleInput } from "../../../add-list-card/components/list-title-input/list-title-input";
import { map, Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { selectUpdateColumnStatus } from './store/column-menu.selectors';

@Component({
  selector: 'app-column-menu',
  imports: [Button, LucideAngularModule, CommonModule, DropdownMenu, Dialog, RemoveColumn, ListTitleInput],
  templateUrl: './column-menu.html',
  styleUrl: './column-menu.css'
})
export class ColumnMenu {
  readonly EllipsisVertical = EllipsisVertical
  readonly Pencil = Pencil
  readonly Trash = Trash

  @Input() column!: ColumnEntity

  openDropdown = signal<boolean>(false)
  openDialog = signal<boolean>(false)
  isEditColumn = signal<boolean>(false)

  isLoading$: Observable<boolean>
  isSuccess$: Observable<boolean>

  constructor(private store: Store) {
    this.isLoading$ = this.store.select(selectUpdateColumnStatus).pipe(map(state => state.isLoading))
    this.isSuccess$ = this.store.select(selectUpdateColumnStatus).pipe(map(state => state.isSuccess))

    this.isSuccess$.subscribe(state => {
      if (state) this.toggleColumnEdit(false)
    })
  }


  toggleDropdown(state?: boolean) {
    this.openDropdown.set(state ?? !this.openDropdown())
  }

  toggleDialog(state?: boolean) {
    this.toggleDropdown(false)
    this.openDialog.set(state ?? !this.openDialog())
  }

  toggleColumnEdit(state?: boolean) {
    this.toggleDropdown(false)
    this.isEditColumn.set(state ?? !this.isEditColumn())
  }
}
