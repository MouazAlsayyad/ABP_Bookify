import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartmentService } from '@proxy/apartments';
import { ApartmentResponse } from '@proxy/apartments/models';

@Component({
  // eslint-disable-next-line @angular-eslint/prefer-standalone
  standalone: false,
  selector: 'app-apartment-detail',
  templateUrl: './apartment-detail.component.html',
  styleUrls: ['./apartment-detail.component.scss']
})
export class ApartmentDetailComponent implements OnInit {
  apartment: ApartmentResponse;
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apartmentService: ApartmentService
  ) {}

  ngOnInit(): void {
    const apartmentId = this.route.snapshot.paramMap.get('id');
    if (apartmentId) {
      this.apartmentService.getApartmentByIdAndCancellationToken(apartmentId, undefined)
        .subscribe(data => {
          this.apartment = data;
          this.isLoading = false;
        });
    }
  }

  navigateToEdit(): void {
    this.router.navigate(['/apartments', this.apartment.id, 'edit']);
  }

  navigateToHome(): void {
    this.router.navigate(['/']);
  }
}
