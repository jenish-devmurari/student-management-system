import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sortDate'
})
export class SortDatePipe implements PipeTransform {

  transform(value: any[], property: string, order: 'asc' | 'desc' = 'desc'): any[] {
    if (!Array.isArray(value)) {
      return value;
    }

    return value.sort((a: any, b: any) => {
      const dateA = new Date(a[property]).getTime();
      const dateB = new Date(b[property]).getTime();
      const comparison = dateA - dateB;
      return order === 'desc' ? -comparison : comparison;
    });
  }

}
