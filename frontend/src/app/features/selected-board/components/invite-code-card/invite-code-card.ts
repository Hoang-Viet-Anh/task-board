import { Component } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { InputComponent } from "@app/shared/components/input/input";
import { Copy, LucideAngularModule } from "lucide-angular";

@Component({
  selector: 'app-invite-code-card',
  imports: [Card, InputComponent, LucideAngularModule],
  templateUrl: './invite-code-card.html',
  styleUrl: './invite-code-card.css'
})
export class InviteCodeCard {
  readonly Copy = Copy
}
