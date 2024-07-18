import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  constructor(private activateRoute : ActivatedRoute){

  }

  ngOnInit() {
    this.activateRoute.fragment.subscribe((section: string | null) => {
      if (section) {
        this.goToSection(section);
      }
    });
  }

  goToSection(section: string = "") {
    if (section) {
      const element = document.getElementById(section);
      if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
      }
    }
  }

}
