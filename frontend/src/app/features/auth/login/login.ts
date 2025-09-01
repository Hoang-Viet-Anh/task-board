import { Component, signal } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { Eye, EyeOff, LoaderCircle, LucideAngularModule } from "lucide-angular";
import { FormControl, ReactiveFormsModule, FormGroup, Validators } from "@angular/forms"
import { InputComponent } from "@app/shared/components/input/input";
import { Button } from "@app/shared/components/button/button";
import { RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, Observable } from 'rxjs';
import { selectLoginStatus } from './store/login.selectors';
import { AsyncPipe, CommonModule } from '@angular/common';
import { performLogin } from './store/login.actions';

@Component({
  selector: 'app-login',
  imports: [LucideAngularModule, Card, ReactiveFormsModule, InputComponent, Button, RouterModule, AsyncPipe, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  readonly EyeOff = EyeOff
  readonly Eye = Eye
  readonly LoaderCircle = LoaderCircle

  loginForm: FormGroup = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  isPasswordVisible = signal<boolean>(false)
  isLoading$: Observable<boolean>
  isSuccess$: Observable<boolean>

  constructor(private readonly store: Store) {
    this.isLoading$ = this.store.select(selectLoginStatus).pipe(map(status => status?.isLoading))
    this.isSuccess$ = this.store.select(selectLoginStatus).pipe(map(status => status?.isSuccess))

    this.isSuccess$.subscribe(success => {
      if (success) this.loginForm.patchValue({
        username: '',
        password: ''
      })
    })
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.store.dispatch(performLogin(this.loginForm.value))
    } else {
      this.loginForm.markAllAsTouched()
    }
  }

  onUsernameChange(value: string) {
    this.loginForm.patchValue({ username: value })
  }

  onPasswordChange(value: string) {
    this.loginForm.patchValue({ password: value })
  }

  togglePasswordVisibility() {
    this.isPasswordVisible.set(!this.isPasswordVisible())
  }

  getErrorField(field: string, validator: string) {
    return this.loginForm.get(field)?.errors?.[validator]
  }
}
