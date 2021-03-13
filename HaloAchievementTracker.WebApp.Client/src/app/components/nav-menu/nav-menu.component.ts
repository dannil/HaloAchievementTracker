import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  
  isExpanded = false;

  collapse(): void {
    this.isExpanded = false;
  }

  toggle(): void {
    this.isExpanded = !this.isExpanded;
  }

}

@NgModule({
  imports: [
    CommonModule,
    RouterModule
  ],
  declarations: [
    NavMenuComponent
  ],
  exports: [
    NavMenuComponent
  ]
})
export class NavMenuModule { }
