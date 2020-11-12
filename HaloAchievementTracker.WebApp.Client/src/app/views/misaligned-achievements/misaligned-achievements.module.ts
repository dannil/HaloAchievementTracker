import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule } from '@app/angular-material.module';
import { MisalignedAchievementsComponent } from '@app/views/misaligned-achievements/misaligned-achievements.component';

@NgModule({
  imports: [
    CommonModule,
    Router,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularMaterialModule
  ],
  declarations: [
    MisalignedAchievementsComponent
  ],
  exports: [
    MisalignedAchievementsComponent
  ]
})
export class MisalignedAchievementsModule { }