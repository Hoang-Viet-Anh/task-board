import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Button } from '@app/shared/components/button/button';
import { Card } from '@app/shared/components/card/card';
import { InputComponent } from '@app/shared/components/input/input';
import { Store } from '@ngrx/store';
import { Eye, EyeOff, LoaderCircle, LucideAngularModule } from 'lucide-angular';
import { map, Observable } from 'rxjs';
import { selectRegisterStatus } from './store/register.selectors';
import { performRegister } from './store/register.actions';
import { passwordsMatchValidator } from './validators/passwords-match.validator';

@Component({
  selector: 'app-registration',
  imports: [LucideAngularModule, Card, ReactiveFormsModule, InputComponent, Button, RouterModule, AsyncPipe, CommonModule],
  templateUrl: './registration.html',
  styleUrl: './registration.css'
})
export class Registration {
  readonly EyeOff = EyeOff
  readonly Eye = Eye
  readonly LoaderCircle = LoaderCircle

  registerForm: FormGroup = new FormGroup({
    username: new FormControl('', [
      Validators.required,
      Validators.minLength(3)
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
    repeatPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ])
  }, { validators: passwordsMatchValidator });

  isPasswordVisible = signal<boolean>(false)
  isRepeatPasswordVisible = signal<boolean>(false)

  isLoading$: Observable<boolean>
  error$: Observable<string | undefined>

  constructor(private readonly store: Store) {
    this.isLoading$ = this.store.select(selectRegisterStatus).pipe(map(status => status?.isLoading))
    this.error$ = this.store.select(selectRegisterStatus).pipe(map(status => status?.error))
  }

  onSubmit() {
    if (this.registerForm.valid) {
      console.log(this.registerForm.value);
      this.store.dispatch(performRegister(this.registerForm.value))
    } else {
      this.registerForm.markAllAsTouched()
    }
  }

  onUsernameChange(value: string) {
    this.registerForm.patchValue({ username: value })
  }

  onPasswordChange(value: string) {
    this.registerForm.patchValue({ password: value })
  }

  onRepeatPasswordChange(value: string) {
    this.registerForm.patchValue({ repeatPassword: value })
  }

  togglePasswordVisibility() {
    this.isPasswordVisible.set(!this.isPasswordVisible())
  }

  toggleRepeatPasswordVisibility() {
    this.isRepeatPasswordVisible.set(!this.isRepeatPasswordVisible())
  }

  getFieldError(field: string, validator: string) {
    return this.registerForm.get(field)?.errors?.[validator]
  }
}
