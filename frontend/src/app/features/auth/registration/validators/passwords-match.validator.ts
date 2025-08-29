import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const passwordsMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const password = control.get('password')?.value;
    const repeatPassword = control.get('repeatPassword')?.value;

    if (password && repeatPassword && password !== repeatPassword) {
        return { passwordsMismatch: true };
    }

    return null;
};