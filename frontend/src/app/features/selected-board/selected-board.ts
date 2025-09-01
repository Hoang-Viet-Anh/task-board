import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Button } from "@app/shared/components/button/button";
import { History, LucideAngularModule } from "lucide-angular";
import { TaskList } from "./components/task-list/task-list";
import { Badge } from "@app/shared/components/badge/badge";

@Component({
  selector: 'app-selected-board',
  imports: [Button, LucideAngularModule, TaskList],
  templateUrl: './selected-board.html',
  styleUrl: './selected-board.css',
  host: {
    class: 'flex flex-col items-center w-full h-full pt-6 gap-10'
  }
})
export class SelectedBoard {
  readonly History = History
  boardId!: string;

  constructor(private readonly route: ActivatedRoute) {
    this.route.paramMap.subscribe(params => {
      this.boardId = params.get('id')!;
    });
  }
}
