import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { History, LucideAngularModule } from "lucide-angular";
import { TaskList } from "./components/task-list/task-list";
import { Store } from '@ngrx/store';
import { getBoardById } from './store/selected-board.actions';
import { map, Observable } from 'rxjs';
import { selectGetColumnsByBoardIdStatus, selectSelectedBoard, selectSelectedBoardColumns } from './store/selected-board.selectors';
import { CommonModule } from '@angular/common';
import { ColumnEntity } from './models/column.model';
import { InviteCodeCard } from "./components/invite-code-card/invite-code-card";
import { AddListCard } from "./components/add-list-card/add-list-card";
import { DragDropModule } from '@angular/cdk/drag-drop';
import { HistoryActions } from "./components/history-actions/history-actions";

@Component({
  selector: 'app-selected-board',
  imports: [LucideAngularModule, TaskList, CommonModule, AddListCard, DragDropModule, HistoryActions, InviteCodeCard],
  templateUrl: './selected-board.html',
  styleUrl: './selected-board.css',
  host: {
    class: 'flex flex-col items-center gap-10'
  }
})
export class SelectedBoard implements OnInit {
  readonly History = History
  boardId!: string;
  boardTitle$: Observable<string | undefined>;
  inviteCode$: Observable<string | undefined>;
  boardColumns$: Observable<ColumnEntity[]>;

  isLoading$: Observable<boolean>

  constructor(
    private readonly route: ActivatedRoute,
    private readonly store: Store
  ) {
    this.route.paramMap.subscribe(params => {
      this.boardId = params.get('id')!;
    });

    this.boardTitle$ = this.store.select(selectSelectedBoard).pipe(map(state => state?.title))
    this.inviteCode$ = this.store.select(selectSelectedBoard).pipe(map(state => state?.inviteCode))
    this.boardColumns$ = this.store.select(selectSelectedBoardColumns)

    this.isLoading$ = this.store.select(selectGetColumnsByBoardIdStatus).pipe(map(state => state.isLoading))
  }

  ngOnInit(): void {
    if (this.boardId)
      this.store.dispatch(getBoardById({ id: this.boardId }))
  }

}
