import { Component } from '@angular/core';
import { Card } from "@app/shared/components/card/card";
import { Store } from '@ngrx/store';
import { Copy, LucideAngularModule } from "lucide-angular";
import { map, Observable } from 'rxjs';
import { selectSelectedBoard } from '../../store/selected-board.selectors';
import { CommonModule } from '@angular/common';
import { Button } from "@app/shared/components/button/button";
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-invite-code-card',
  imports: [Card, LucideAngularModule, CommonModule, Button],
  templateUrl: './invite-code-card.html',
  styleUrl: './invite-code-card.css'
})
export class InviteCodeCard {
  readonly Copy = Copy

  inviteCode$: Observable<string | undefined>

  constructor(
    private store: Store,
    private messageService: MessageService
  ) {
    this.inviteCode$ = this.store.select(selectSelectedBoard).pipe(map(b => b?.inviteCode))
  }

  onCopy(code: string) {
    navigator.clipboard.writeText(code)
    this.messageService.add({
      summary: "Invite code copied",
      severity: "success"
    })
  }
}
