import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConstantsService {

  baseUrl = ''; // development
  resourceUrl = '';
  company = 'Nesadi Sacco';
  countryId = 1;

  constructor() {
    if (environment.production) {
      this.baseUrl = 'https://mpbackend.nesadisacco.com/api/';
      this.resourceUrl = 'https://mpbackend.nesadisacco.com/';
    } else {
      this.baseUrl = 'https://localhost:44353/api/';
      this.resourceUrl = 'https://localhost:44353/';
    }
  }
}
