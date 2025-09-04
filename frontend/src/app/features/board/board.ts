import { Component, OnInit } from '@angular/core';
import { LucideAngularModule } from "lucide-angular";
import { AddBoardButton } from "./components/add-board-button/add-board-button";
import { Store } from '@ngrx/store';
import { getBoards } from './store/board.actions';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { BoardEntity } from './models/board.model';
import { selectBoards } from './store/board.selectors';
import { BoardCard } from "./components/board-card/board-card";

@Component({
  selector: 'app-board',
  imports: [LucideAngularModule, AddBoardButton, CommonModule, BoardCard],
  templateUrl: './board.html',
  styleUrl: './board.css',
  host: {
    class: 'flex flex-col w-full h-full overflow-y-auto items-center'
  }
})
export class Board implements OnInit {
  boards$: Observable<BoardEntity[]>

  constructor(
    private readonly store: Store
  ) {
    this.boards$ = this.store.select(selectBoards)
  }

  ngOnInit(): void {
    this.store.dispatch(getBoards());
  }
}
