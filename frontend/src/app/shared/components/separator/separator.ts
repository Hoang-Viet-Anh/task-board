import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-separator',
  imports: [CommonModule],
  templateUrl: './separator.html',
  styleUrl: './separator.css',
})
export class Separator {
  @Input() orientation: 'vertical' | 'horizontal' = 'horizontal';
  @Input() class: string = '';
}
