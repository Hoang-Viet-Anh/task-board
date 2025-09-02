import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, EventEmitter, Inject, Input, OnDestroy, Output, PLATFORM_ID, signal, ViewChild } from '@angular/core';
import { Card } from "../card/card";
import { computePosition, flip, offset, autoUpdate } from '@floating-ui/dom';

@Component({
  selector: 'app-dropdown-menu',
  imports: [CommonModule, Card],
  templateUrl: './dropdown-menu.html',
  styleUrl: './dropdown-menu.css'
})
export class DropdownMenu implements AfterViewInit, OnDestroy {
  @Input() open: boolean = false;
  @Output() onClose = new EventEmitter<void>();
  @ViewChild('trigger') trigger!: ElementRef<HTMLDivElement>;
  @ViewChild('dropdown') dropdown!: ElementRef<HTMLDivElement>;

  placement: 'top' | 'bottom' = 'bottom';

  private cleanup?: () => void;

  constructor() { }

  ngAfterViewInit(): void {
    this.cleanup = autoUpdate(
      this.trigger.nativeElement,
      this.dropdown.nativeElement,
      () => this.updatePosition()
    );
  }

  ngOnDestroy(): void {
    this.cleanup?.();
  }

  closeDropdown() {
    this.onClose.emit();
  }

  onDropdownClick(event: Event) {
    event.stopPropagation();
  }

  updatePosition() {
    computePosition(this.trigger.nativeElement, this.dropdown.nativeElement, {
      placement: this.placement,
      middleware: [offset(5), flip()]
    }).then(({ x, y, placement }) => {
      this.placement = placement.startsWith('top') ? 'top' : 'bottom';

      Object.assign(this.dropdown.nativeElement.style, {
        left: `${x}px`,
        top: `${y}px`
      })
    })
  }
}
