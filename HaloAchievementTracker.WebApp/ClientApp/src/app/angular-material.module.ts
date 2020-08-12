import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  MatButtonModule,
  MatBadgeModule,
  MatInputModule,
  MatProgressSpinnerModule,
} from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    MatButtonModule,
    MatBadgeModule,
    MatInputModule,
    MatProgressSpinnerModule
  ],
  exports: [
    MatButtonModule,
    MatBadgeModule,
    MatInputModule,
    MatProgressSpinnerModule
  ]
})

export class AngularMaterialModule { }
