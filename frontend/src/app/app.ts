import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { Dialog } from "./shared/components/dialog/dialog";
import { Drawer } from "./shared/components/drawer/drawer";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule, Dialog, Drawer],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'frontend';
}
