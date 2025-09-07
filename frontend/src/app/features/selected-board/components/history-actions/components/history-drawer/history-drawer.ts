import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Log } from '@app/features/selected-board/models/log.model';
import { loadMoreLogs } from '@app/features/selected-board/store/selected-board.actions';
import { selectSelectedBoardLogs } from '@app/features/selected-board/store/selected-board.selectors';
import { Button } from "@app/shared/components/button/button";
import { DrawerService } from '@app/shared/services/drawer.service';
import { Store } from '@ngrx/store';
import { Dot, LucideAngularModule, RotateCcw, X } from "lucide-angular";
import { Observable } from 'rxjs';
import { toHTML } from 'slack-markdown'

@Component({
  selector: 'app-history-drawer',
  imports: [Button, LucideAngularModule, CommonModule],
  templateUrl: './history-drawer.html',
  styleUrl: './history-drawer.css',
  host: {
    class: 'flex flex-col h-full'
  }
})
export class HistoryDrawer {
  readonly X = X
  readonly Dot = Dot
  readonly Refresh = RotateCcw

  @Input() boardId!: string;
  logs$: Observable<Log[]>

  constructor(
    private drawerService: DrawerService,
    private store: Store,
  ) {
    this.logs$ = this.store.select(selectSelectedBoardLogs)
  }

  closeDrawer() {
    this.drawerService.close()
  }

  onLoadLogs() {
    this.store.dispatch(loadMoreLogs({ id: this.boardId }));
  }

  parseSlackMarkdown(log: string) {
    return toHTML(log)
  }
}
