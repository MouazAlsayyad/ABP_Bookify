import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApartmentDetailComponent } from './apartment-detail/apartment-detail.component';
import { ApartmentFormComponent } from './apartment-form/apartment-form.component';

const routes: Routes = [
  {
    path: 'new',
    component: ApartmentFormComponent,
  },
  {
    path: ':id',
    component: ApartmentDetailComponent,
  },
  {
    path: ':id/edit',
    component: ApartmentFormComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ApartmentRoutingModule {} 