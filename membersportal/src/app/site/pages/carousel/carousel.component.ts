import { Component, OnInit } from '@angular/core';
import { OwlOptions } from 'ngx-owl-carousel-o';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.css']
})
export class CarouselComponent implements OnInit {

  customOptions: OwlOptions = {
    loop: true,
    autoplay: true,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: true,
    dots: true,
    navSpeed: 700,
    navText: ['', ''],
    responsive: {
      0: {
        items: 1
      },
      400: {
        items: 1
      },
      740: {
        items: 1
      },
      940: {
        items: 1
      }
    },
    nav: false,
    animateOut: true,
    animateIn: true
  };

  slidesStore = [
    {id: '0', src: '/assets/images/bg-img/1.jpg', alt: '', title: 'Test'},
    {id: '1', src: '/assets/images/bg-img/2.jpg', alt: '', title: 'Test 2'},
    {id: '2', src: '/assets/images/bg-img/3.jpg', alt: '', title: 'Test 3'}
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
