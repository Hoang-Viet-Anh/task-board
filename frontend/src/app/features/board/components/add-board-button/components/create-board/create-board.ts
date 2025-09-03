import { Component, OnInit } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { LoaderCircle, LucideAngularModule } from "lucide-angular";
import { CommonModule } from '@angular/common';
import { InputComponent } from "@app/shared/components/input/input";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { createBoard } from '../../store/add-board.actions';
import { map, Observable } from 'rxjs';
import { selectAddBoardStatus } from '../../store/add-board.selectors';

@Component({
  selector: 'app-create-board',
  imports: [Button, CommonModule, LucideAngularModule, InputComponent, ReactiveFormsModule],
  templateUrl: './create-board.html',
  styleUrl: './create-board.css'
})
export class CreateBoard implements OnInit {
  readonly LoaderCircle = LoaderCircle

  createBoardForm: FormGroup = new FormGroup({
    boardTitle: new FormControl('', Validators.required)
  })

  isLoading$: Observable<boolean>;

  constructor(private readonly store: Store) {
    this.isLoading$ = this.store.select(selectAddBoardStatus).pipe(map(status => status?.isLoading))
  }

  ngOnInit(): void {
    this.createBoardForm.patchValue({
      boardTitle: ''
    })
  }

  onCreateSubmit() {
    if (this.createBoardForm.valid) {
      this.store.dispatch(createBoard(this.createBoardForm.value));
    } else {
      this.createBoardForm.markAsTouched()
    }
  }

  onBoardTitleChange(value: string) {
    this.createBoardForm.patchValue({ boardTitle: value })
  }
}
