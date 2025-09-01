import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { LucideAngularModule, Plus } from "lucide-angular";
import { Button } from "@app/shared/components/button/button";

@Component({
  selector: 'app-add-task-button',
  imports: [Card, CommonModule, LucideAngularModule, Button],
  templateUrl: './add-task-button.html',
  styleUrl: './add-task-button.css',
  host: {
    class: 'w-full'
  }
})
export class AddTaskButton {
  readonly Plus = Plus
}
