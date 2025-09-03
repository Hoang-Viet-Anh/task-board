import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DialogLayout } from "../dialog-layout/dialog-layout";
import { Button } from "@app/shared/components/button/button";
import { DialogService } from '@app/shared/services/dialog.service';
import { LoaderCircle, LucideAngularModule } from "lucide-angular";

@Component({
  selector: 'app-delete-dialog',
  imports: [DialogLayout, Button, LucideAngularModule],
  templateUrl: './delete-dialog.html',
  styleUrl: './delete-dialog.css'
})
export class DeleteDialog {
  readonly LoaderCircle = LoaderCircle

  @Input() title: string = 'Remove Item'
  @Input() description: string = 'Are you sure you want to remove this item? This action cannot be undone.'
  @Input() isLoading: boolean = false

  @Output() onDelete: () => void = () => { };

  constructor(private dialogService: DialogService) { }

  onClose() {
    this.dialogService.close()
  }
}
