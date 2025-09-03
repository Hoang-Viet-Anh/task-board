import { Component, Input } from '@angular/core';
import { History, LucideAngularModule } from "lucide-angular";
import { Button } from "@app/shared/components/button/button";
import { DrawerService } from '@app/shared/services/drawer.service';
import { HistoryDrawer } from './components/history-drawer/history-drawer';

@Component({
  selector: 'app-history-actions',
  imports: [LucideAngularModule, Button],
  templateUrl: './history-actions.html',
  styleUrl: './history-actions.css'
})
export class HistoryActions {
  readonly History = History

  @Input() boardId!: string

  constructor(private drawerService: DrawerService) { }

  onDrawerOpen() {
    this.drawerService.open(HistoryDrawer, {
      boardId: this.boardId
    })
  }
}
