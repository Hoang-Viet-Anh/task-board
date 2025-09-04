import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Button } from "@app/shared/components/button/button";
import { LogOut, LucideAngularModule } from "lucide-angular";
import { RouterModule } from "@angular/router";
import { Store } from '@ngrx/store';
import { logoutAction } from './store/header.actions';

@Component({
  selector: 'app-header',
  imports: [Button, CommonModule, LucideAngularModule, RouterModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
  host: { class: 'flex flex-col w-screen h-screen overflow-hidden' }
})
export class HeaderLayout {
  readonly Logout = LogOut

  constructor(private readonly store: Store) { }

  onLogout() {
    this.store.dispatch(logoutAction());
  }
}
