import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { AchievementsService } from '@app/services/achievements.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AngularMaterialModule } from '@app/angular-material.module';

@Component({
  selector: 'app-list-achievements',
  templateUrl: './list-achievements.component.html'
})
export class ListAchievementsComponent {

  achievements$: Observable<IAchievement[]>;

  constructor(private achievementsService: AchievementsService) {
    this.achievements$ = this.achievementsService.getList();
  }

}

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    AngularMaterialModule
  ],
  declarations: [
    ListAchievementsComponent
  ],
  exports: [
    ListAchievementsComponent
  ]
})
export class ListAchievementsModule { }