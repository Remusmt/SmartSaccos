import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConstantsService {

  baseUrl = 'https://localhost:44383/api/'; // development

  constructor() {
    if (environment.production) {
      this.baseUrl = 'http://169.239.169.186:81/api/';
    } else {
      this.baseUrl = 'https://localhost:44383/api/';
    }
  }
}
