import { Pipe, PipeTransform } from '@angular/core';
import { Amenity } from '@proxy/apartments';

@Pipe({
  name: 'amenity',
  standalone: true 
})
export class AmenityPipe implements PipeTransform {
  transform(value: Amenity): string {
    return Amenity[value];
  }
} 