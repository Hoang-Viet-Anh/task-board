import { CommonModule, } from '@angular/common';
import { AfterViewInit, Component, ViewChild, ViewContainerRef } from '@angular/core';
import { DialogService } from '@app/shared/services/dialog.service';

@Component({
  selector: 'app-dialog',
  imports: [CommonModule],
  templateUrl: './dialog.html',
  styleUrl: './dialog.css'
})
export class Dialog implements AfterViewInit {
  @ViewChild('dialogContainer', { read: ViewContainerRef }) container!: ViewContainerRef;

  constructor(private dialogService: DialogService) { }

  ngAfterViewInit(): void {
    this.dialogService.registerContainer(this.container);
  }

  isOpen() {
    return this.dialogService.isOpen();
  }

  closeDialog(event: Event) {
    event.stopPropagation()
    this.dialogService.close();
  }

  onDialogClick(event: Event) {
    event.stopPropagation();
  }
}
