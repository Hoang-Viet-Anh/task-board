import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { Store } from '@ngrx/store';
import { firstValueFrom, map, Observable } from 'rxjs';
import { LoaderCircle, LucideAngularModule, Trash } from "lucide-angular";
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { selectRemoveColumnStatus } from '../../store/column-menu.selectors';
import { removeColumnRequest } from '../../store/column-menu.actions';
import { DialogService } from '@app/shared/services/dialog.service';
import { DeleteDialog } from '@app/shared/modals/delete-dialog/delete-dialog';

@Component({
  selector: 'app-remove-column',
  imports: [Button, LucideAngularModule, CommonModule],
  templateUrl: './remove-column.html',
  styleUrl: './remove-column.css'
})
export class RemoveColumn {
  readonly LoaderCircle = LoaderCircle
  readonly Trash = Trash

  isLoading$: Observable<boolean>
  @Input() column!: ColumnEntity

  constructor(
    private store: Store,
    private dialogService: DialogService
  ) {
    this.isLoading$ = this.store.select(selectRemoveColumnStatus).pipe(map(status => status.isLoading))
  }

  onRemoveList() {
    this.store.dispatch(removeColumnRequest(this.column))
  }

  async onRemoveDialogOpen() {
    this.dialogService.open(DeleteDialog, {
      title: 'Remove List',
      description: 'Are you sure you want to delete this List? This action cannot be undone.',
      isLoading: await firstValueFrom(this.isLoading$),
      onDelete: () => this.onRemoveList()
    })
  }
}
