import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ViewChild, ViewContainerRef } from '@angular/core';
import { DrawerService } from '@app/shared/services/drawer.service';
import { LucideAngularModule, X } from "lucide-angular";

@Component({
  selector: 'app-drawer',
  imports: [CommonModule, LucideAngularModule],
  templateUrl: './drawer.html',
  styleUrl: './drawer.css'
})
export class Drawer implements AfterViewInit {
  @ViewChild('drawerContainer', { read: ViewContainerRef }) container!: ViewContainerRef;
  readonly X = X

  constructor(private drawerService: DrawerService) { }

  ngAfterViewInit(): void {
    this.drawerService.registerContainer(this.container);
  }

  isOpen() {
    return this.drawerService.isOpen()
  }

  closeDrawer(event: Event) {
    event.stopPropagation()
    this.drawerService.close()
  }

  preventDrawerClose(event: Event) {
    event.stopPropagation()
  }
}
