import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { History, LucideAngularModule } from "lucide-angular";
import { TaskList } from "./components/task-list/task-list";
import { Store } from '@ngrx/store';
import { getBoardById, getColumnsByBoardId, getColumnsByBoardIdSuccess } from './store/selected-board.actions';
import { combineLatest, map, Observable, take, tap } from 'rxjs';
import { selectGetColumnsByBoardIdStatus, selectSelectedBoard, selectSelectedBoardColumns } from './store/selected-board.selectors';
import { CommonModule } from '@angular/common';
import { ColumnEntity } from './models/column.model';
import { InviteCodeCard } from "./components/invite-code-card/invite-code-card";
import { AddListCard } from "./components/add-list-card/add-list-card";
import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { HistoryActions } from "./components/history-actions/history-actions";
import { moveColumnRequest } from './components/task-list/components/column-menu/store/column-menu.actions';
import { DialogService } from '@app/shared/services/dialog.service';
import { Actions, ofType } from '@ngrx/effects';
import { TaskDialog } from './components/task-list/components/task-card/components/task-dialog/task-dialog';
import { generateKeyBetween } from "fractional-indexing";

@Component({
  selector: 'app-selected-board',
  imports: [LucideAngularModule, TaskList, CommonModule, AddListCard, DragDropModule, HistoryActions, InviteCodeCard],
  templateUrl: './selected-board.html',
  styleUrl: './selected-board.css',
  host: {
    class: 'flex h-full'
  }
})
export class SelectedBoard implements OnInit {
  readonly History = History
  boardId: string = '';
  taskId: string = '';
  boardTitle$: Observable<string | undefined>;
  inviteCode$: Observable<string | undefined>;
  boardColumns$: Observable<ColumnEntity[]>;

  isLoading$: Observable<boolean>

  constructor(
    private route: ActivatedRoute,
    private store: Store,
    private dialogService: DialogService,
    private actions$: Actions,
    private router: Router
  ) {
    combineLatest([
      this.route.paramMap,
      this.route.parent!.paramMap
    ]).subscribe(([childParams, parentParams]) => {
      this.boardId = parentParams.get('boardId') ?? '';
      this.taskId = childParams.get('taskId') ?? '';
    });

    this.boardTitle$ = this.store.select(selectSelectedBoard).pipe(map(state => state?.title))
    this.inviteCode$ = this.store.select(selectSelectedBoard).pipe(map(state => state?.inviteCode))
    this.boardColumns$ = this.store.select(selectSelectedBoardColumns)

    this.isLoading$ = this.store.select(selectGetColumnsByBoardIdStatus).pipe(map(state => state.isLoading))
  }

  ngOnInit(): void {
    if (this.boardId)
      this.store.dispatch(getBoardById({ id: this.boardId }))
    if (this.taskId) {
      this.actions$.pipe(
        ofType(getColumnsByBoardIdSuccess),
        take(1),
        tap(() => {
          this.dialogService.open(TaskDialog, {
            taskId: this.taskId
          })
          this.router.navigate([`board/${this.boardId}`])
        })
      ).subscribe()
    }
  }

  onColumnDrop(event: CdkDragDrop<any>) {
    const columns: ColumnEntity[] = event.container.data
    const previousIndex = event.previousIndex
    const currentIndex = event.currentIndex

    if (previousIndex === currentIndex)
      return

    let currentList = [...columns ?? []]

    moveItemInArray(currentList, previousIndex, currentIndex)

    const leftItem = currentList[currentIndex - 1]
    const currentItem = currentList[currentIndex]
    const rightItem = currentList[currentIndex + 1]

    let newOrderValue = generateKeyBetween(leftItem?.order, rightItem?.order)


    const changedColumn: ColumnEntity = {
      ...currentItem,
      order: newOrderValue,
      title: undefined
    }

    this.store.dispatch(moveColumnRequest({ columns: currentList, changedColumn }))
  }
}
