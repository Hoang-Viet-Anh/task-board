import { Component, Input } from '@angular/core';
import { Button } from '@app/shared/components/button/button';
import { Card } from '@app/shared/components/card/card';
import { DialogService } from '@app/shared/services/dialog.service';
import { LucideAngularModule, X } from "lucide-angular";

@Component({
  selector: 'app-dialog-layout',
  imports: [Button, LucideAngularModule, Card],
  templateUrl: './dialog-layout.html',
  styleUrl: './dialog-layout.css'
})
export class DialogLayout {
  readonly X = X;

  @Input() cardClass: string = '';
  @Input() closeButtonClass: string = '';

  constructor(private dialogService: DialogService) { }

  closeDialog() {
    this.dialogService.close()
  }
}
