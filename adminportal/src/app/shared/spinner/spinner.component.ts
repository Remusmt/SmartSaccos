import { Component, OnInit } from '@angular/core';
import { SpinnerService } from '../services/spinner.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent implements OnInit {

  showSpinner = false;
  constructor(private spinnerService: SpinnerService) { }

  ngOnInit(): void {
    this.spinnerService.visibility.subscribe(
      res => {
        this.showSpinner = res;
      }
    );
  }
}
