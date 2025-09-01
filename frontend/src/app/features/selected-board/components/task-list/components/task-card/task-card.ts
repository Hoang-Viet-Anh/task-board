import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { Button } from "@app/shared/components/button/button";
import { Calendar, Dot, EllipsisVertical, LucideAngularModule } from "lucide-angular";
import { Badge } from "@app/shared/components/badge/badge";
import { ChangeListDropdown } from "../change-list-dropdown/change-list-dropdown";

@Component({
  selector: 'app-task-card',
  imports: [Card, CommonModule, Button, LucideAngularModule, Badge, ChangeListDropdown],
  templateUrl: './task-card.html',
  styleUrl: './task-card.css'
})
export class TaskCard {
  readonly EllipsisVertical = EllipsisVertical
  readonly Calendar = Calendar
  readonly Dot = Dot
}
