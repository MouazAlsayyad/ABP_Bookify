import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ApartmentDetailComponent } from './apartment-detail/apartment-detail.component';
import { SharedModule } from '../shared/shared.module';
import { ApartmentRoutingModule } from './apartment-routing.module';
import { ApartmentFormComponent } from './apartment-form/apartment-form.component';

@NgModule({
  declarations: [ApartmentDetailComponent, ApartmentFormComponent],
  imports: [
    CommonModule,
    ApartmentRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class ApartmentModule { } 