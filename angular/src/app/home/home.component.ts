import { AuthService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { ApartmentService, SearchApartmentResponse } from '../proxy/apartments';
import { SearchApartmentDto } from '../proxy/apartments/search-apartments/models';
import { finalize } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  // eslint-disable-next-line @angular-eslint/prefer-standalone
  standalone: false,
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  apartments: SearchApartmentResponse[] = [];
  isLoading = false;

  // Search and pagination properties
  searchDto: SearchApartmentDto = { page: 1, pageSize: 6 }; // Default page size to 6 for better card display
  searchKey: string = '';
  startDate?: string;
  endDate?: string;
  today: string; // Added today property
  // Assuming the API returns total count or we manage it based on results length for simplicity here
  // For a robust pagination, totalCount should come from the backend.
  // Let's simulate totalPages based on whether we get a full page of results or not for now.
  totalPages = 1;
  currentPage = 1;

  constructor(
    private authService: AuthService,
    private apartmentService: ApartmentService,
    private router: Router
  ) {
    const now = new Date();
    this.today = now.toISOString().split('T')[0]; // Set today's date in YYYY-MM-DD format
  }

  ngOnInit(): void {
    this.applyFilters(); // Initial fetch with default page 1
  }

  applyFilters(): void {
    if (this.startDate && this.endDate && this.endDate < this.startDate) {
      console.error('End date cannot be before start date.');
      this.endDate = undefined;
    }
    this.currentPage = 1; // Reset to page 1 for new search/filter
    this.totalPages = 1;  // Reset totalPages, will be determined by fetch
    this.searchDto.page = this.currentPage;
    this.searchDto.searchKey = this.searchKey || undefined;
    this.searchDto.startDate = this.startDate || undefined;
    this.searchDto.endDate = this.endDate || undefined;
    this.fetchApartments();
  }

  fetchApartments(): void {
    this.isLoading = true;
    this.apartmentService.searchApartmentsByInputAndCancellationToken(this.searchDto)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe(data => {
        this.apartments = data;
        if (data.length < this.searchDto.pageSize) {
          this.totalPages = this.currentPage; // This is the last page
        } else {
          // Full page, so there might be more. Set totalPages to allow navigating to the next one.
          this.totalPages = this.currentPage + 1; 
        }
      });
  }

  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  login() {
    this.authService.navigateToLogin();
  }

  viewApartment(apartmentId: string) {
    this.router.navigate(['/apartments', apartmentId]);
  }

  onSearchKeyChange(value: string): void {
    this.searchKey = value;
  }

  onStartDateChange(value: string): void {
    this.startDate = value;
    if (this.endDate && this.startDate && this.endDate < this.startDate) {
      this.endDate = undefined;
    }
  }

  onEndDateChange(value: string): void {
    this.endDate = value;
    if (this.startDate && this.endDate && this.endDate < this.startDate) {
        console.error('End date cannot be before start date.');
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.searchDto.page = this.currentPage;
      this.fetchApartments();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.searchDto.page = this.currentPage;
      this.fetchApartments();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.searchDto.page = this.currentPage;
      this.fetchApartments();
    }
  }

  get pageNumbers(): number[] { // Getter for page numbers array
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  navigateToCreate(): void {
    this.router.navigate(['/apartments/new']);
  }
}
