import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-avator',
  templateUrl: './avator.component.html',
  styleUrls: ['./avator.component.css']
})
export class AvatorComponent {

  @Input() public initials = '';
  @Input() public avatorUrl = '';
  showIntials = false;
  constructor() { }
}
