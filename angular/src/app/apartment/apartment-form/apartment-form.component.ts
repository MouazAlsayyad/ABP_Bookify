import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartmentService } from '@proxy/apartments';
import { Amenity } from '@proxy/apartments';
import { ApartmentCreateUpdateDto } from '@proxy/apartments/create-apartment/models';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
  // eslint-disable-next-line @angular-eslint/prefer-standalone
  standalone:false,
  selector: 'app-apartment-form',
  templateUrl: './apartment-form.component.html',
  styleUrls: ['./apartment-form.component.scss']
})
export class ApartmentFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  apartmentId: string;
  amenities = Object.keys(Amenity).filter(k => !isNaN(Number(Amenity[k]))).map(k => ({ id: Amenity[k], name: k }));

  constructor(
    private fb: FormBuilder,
    private apartmentService: ApartmentService,
    private route: ActivatedRoute,
    private router: Router,
    private toaster: ToasterService
  ) {}

  ngOnInit(): void {
    this.apartmentId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.apartmentId;
    this.buildForm();

    if (this.isEditMode) {
      this.apartmentService.getApartmentByIdAndCancellationToken(this.apartmentId, undefined).subscribe(apartment => {
        this.form.patchValue(apartment);
        this.form.patchValue({
          priceAmount: apartment.price,
        });
        this.setAmenities(apartment.amenities);
      });
    }
  }

  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      address: this.fb.group({
        country: ['', Validators.required],
        state: ['', Validators.required],
        zipCode: ['', Validators.required],
        city: ['', Validators.required],
        street: ['', Validators.required]
      }),
      priceAmount: [0, [Validators.required, Validators.min(0)]],
      cleaningFeeAmount: [0, [Validators.required, Validators.min(0)]],
      currency: ['USD', Validators.required],
      amenities: this.fb.array([])
    });
  }

  get amenitiesFormArray() {
    return this.form.get('amenities') as FormArray;
  }

  onCheckboxChange(e) {
    const amenities: FormArray = this.form.get('amenities') as FormArray;

    if (e.target.checked) {
      amenities.push(this.fb.control(e.target.value));
    } else {
       const index = amenities.controls.findIndex(x => x.value === e.target.value);
       amenities.removeAt(index);
    }
  }

  setAmenities(amenities: Amenity[]) {
    const amenityFGs = amenities.map(amenity => this.fb.control(amenity));
    const amenityFormArray = this.fb.array(amenityFGs);
    this.form.setControl('amenities', amenityFormArray);
  }

  isAmenitySelected(amenityId: number): boolean {
    return this.amenitiesFormArray.value.some(id => id === amenityId);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    const formValue = this.form.value as ApartmentCreateUpdateDto;

    const request = this.isEditMode
      ? this.apartmentService.updateApartmentByIdAndRequestAndCancellationToken(this.apartmentId, formValue, undefined)
      : this.apartmentService.createApartmentByRequestAndCancellationToken(formValue, undefined);

    request.subscribe(result => {
      const message = this.isEditMode ? 'Apartment updated successfully!' : 'Apartment created successfully!';
      this.toaster.success(message);
      const id = this.isEditMode ? this.apartmentId : result;
      this.router.navigate(['/apartments', id]);
    });
  }
}
