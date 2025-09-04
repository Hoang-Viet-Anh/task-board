import { ComponentRef, Injectable, signal, Type, ViewContainerRef } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class DrawerService {
    private container?: ViewContainerRef;
    private componentRef?: ComponentRef<any>;
    private _isOpen = signal(false);

    registerContainer(container: ViewContainerRef) {
        this.container = container;
    }

    isOpen() {
        return this._isOpen();
    }

    open<T>(component: Type<T>, data?: Partial<T>) {
        if (!this.container) return;

        this.container.clear();
        this.componentRef = this.container.createComponent(component);

        if (data) Object.assign(this.componentRef.instance, data);

        this._isOpen.set(true);
    }

    close() {
        this._isOpen.set(false);
    }
}