import { Component, EventEmitter, Input, OnInit, Output, signal } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { CommonModule } from '@angular/common';
import { LoaderCircle, LucideAngularModule, Plus } from "lucide-angular";
import { InputComponent } from "@app/shared/components/input/input";
import { Store } from '@ngrx/store';
import { addListRequest } from '../../store/add-list.actions';
import { ColumnEntity } from '@app/features/selected-board/models/column.model';
import { updateColumnRequest } from '../../../task-list/components/column-menu/store/column-menu.actions';

@Component({
  selector: 'app-list-title-input',
  imports: [InputComponent, Button, LucideAngularModule, CommonModule],
  templateUrl: './list-title-input.html',
  styleUrl: './list-title-input.css',
  host: {
    class: 'flex flex-col gap-2'
  }
})
export class ListTitleInput implements OnInit {
  readonly Plus = Plus
  readonly LoaderCircle = LoaderCircle

  @Input() column!: ColumnEntity;
  @Input() isLoading: boolean = false
  @Output() toggleShowInput = new EventEmitter<boolean>();

  listTitle = signal<string>('')

  constructor(private store: Store) {
  }

  ngOnInit(): void {
    if (this.column.title)
      this.listTitle.set(this.column.title)
  }

  onTitleChange(value: string) {
    this.listTitle.set(value)
  }

  onInputClose(state: boolean) {
    if (this.column.title)
      this.listTitle.set(this.column.title)
    else
      this.listTitle.set('')

    this.toggleShowInput.emit(state);
  }

  createList() {
    if (this.listTitle().trim().length === 0) {
      this.onInputClose(false)
      return;
    }
    let id = this.column.id

    if (id)
      this.store.dispatch(updateColumnRequest({
        id: this.column.id,
        title: this.listTitle(),
        boardId: this.column.boardId,
      }))
    else
      this.store.dispatch(addListRequest({
        title: this.listTitle(),
        boardId: this.column.boardId,
      }))
  }
}
