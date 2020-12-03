import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConstantsService {

  baseUrl = 'https://localhost:44365/api/'; // development
  company = 'Nesadi Sacco';
  countryId = 1;

  constructor() {
    if (environment.production) {
      this.baseUrl = 'https://mpbackend.nesadisacco.com/api/';
    } else {
      this.baseUrl = 'https://localhost:44365/api/';
    }
  }
}
