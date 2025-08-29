import { Component } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { Button } from "@app/shared/components/button/button";
import { LucideAngularModule } from "lucide-angular";
import { AddBoardButton } from "./components/add-board-button/add-board-button";

@Component({
  selector: 'app-board',
  imports: [Card, Button, LucideAngularModule, AddBoardButton],
  templateUrl: './board.html',
  styleUrl: './board.css',
  host: {
    class: 'flex flex-col w-full h-full overflow-y-auto items-center'
  }
})
export class Board {
}
