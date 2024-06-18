import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormControl, FormControlDirective, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {

  constructor() { }

  public validationNgClass(control: AbstractControl | null): { [key: string]: boolean | undefined } {
    return {
      'error': control?.invalid && (control?.dirty || control?.touched),
      'success': control?.valid
    };
  }

  public isFormFieldInvalid(control: AbstractControl | null): boolean {
    return control?.errors?.['required'] && (control?.dirty || control?.touched);
  }

  public isEmailPatternMatch(control: AbstractControl | null): boolean {
    return control?.errors?.['pattern'] && (control?.dirty || control?.touched);
  }

  // Validator for positive input
  public positiveNumberValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const value = control.value;
    return value >= 0 ? null : { 'negativeNumber': true };
  }

  // Validator to check if date is not in the future
  public notFutureDateValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const value = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return value <= today ? null : { 'futureDate': true };
  }

  // Custom validator function to ensure that the date of birth is before the date of enrollment
  public dateOfBirthBeforeEnrollmentValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      const dateOfBirth = control.get('dateOfBirth')?.value;
      const enrollmentDate = control.get('dateOfEnrollment')?.value;
      if (dateOfBirth && enrollmentDate && new Date(dateOfBirth) > new Date(enrollmentDate)) {
        return { 'dateOfBirthAfterEnrollment': true };
      }
      if (dateOfBirth && enrollmentDate && new Date(dateOfBirth) > new Date(enrollmentDate)) {
        return { 'dateOfBirthAfterEnrollment': true };
      }
      return null;
    };
  }

}
