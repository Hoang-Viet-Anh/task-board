import { Component } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { LoaderCircle, LucideAngularModule } from "lucide-angular";
import { CommonModule } from '@angular/common';
import { InputComponent } from "@app/shared/components/input/input";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { joinBoard } from '../../store/add-board.actions';
import { map, Observable } from 'rxjs';
import { selectJoinBoardStatus } from '../../store/add-board.selectors';


@Component({
  selector: 'app-join-board',
  imports: [Button, CommonModule, LucideAngularModule, InputComponent, ReactiveFormsModule],
  templateUrl: './join-board.html',
  styleUrl: './join-board.css'
})
export class JoinBoard {
  readonly LoaderCircle = LoaderCircle

  joinBoardForm: FormGroup = new FormGroup({
    inviteCode: new FormControl('', Validators.required)
  })

  isLoading$: Observable<boolean>;
  error$: Observable<string | undefined>;

  constructor(private readonly store: Store) {
    this.isLoading$ = this.store.select(selectJoinBoardStatus).pipe(map(status => status?.isLoading))
    this.error$ = this.store.select(selectJoinBoardStatus).pipe(map(status => status?.error))
  }

  onJoinSubmit() {
    if (this.joinBoardForm.valid) {
      this.store.dispatch(joinBoard(this.joinBoardForm.value))
    } else {
      this.joinBoardForm.markAsTouched()
    }
  }

  onInviteCodeChange(value: string) {
    this.joinBoardForm.patchValue({ inviteCode: value })
  }
}
