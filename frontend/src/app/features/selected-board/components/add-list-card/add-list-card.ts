import { Component, Input, signal } from '@angular/core';
import { Separator } from "@app/shared/components/separator/separator";
import { Button } from "@app/shared/components/button/button";
import { CommonModule } from '@angular/common';
import { LoaderCircle, LucideAngularModule, Plus } from "lucide-angular";
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { selectAddListStatus } from './store/add-list.selectors';
import { addListRequest } from './store/add-list.actions';
import { ListTitleInput } from "./components/list-title-input/list-title-input";

@Component({
  selector: 'app-add-list-card',
  imports: [Separator, Button, CommonModule, LucideAngularModule, ListTitleInput],
  templateUrl: './add-list-card.html',
  styleUrl: './add-list-card.css'
})
export class AddListCard {
  readonly Plus = Plus
  readonly LoaderCircle = LoaderCircle

  @Input() boardId!: string

  showInput = signal<boolean>(false)

  isLoading$: Observable<boolean>
  isSuccess$: Observable<boolean>

  constructor(private store: Store) {
    this.isLoading$ = this.store.select(selectAddListStatus).pipe(map(state => state.isLoading))
    this.isSuccess$ = this.store.select(selectAddListStatus).pipe(map(state => state.isSuccess))

    this.isSuccess$.subscribe(state => {
      if (state)
        this.toggleShowInput(false)
    })
  }

  toggleShowInput(state?: boolean) {
    this.showInput.set(state ?? !this.showInput())
  }
}
