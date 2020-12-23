import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { combineLatest, Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
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

  filter$: Observable<string>;
  filterForm: FormGroup;

  achievements$: Observable<IAchievement[]>;
  achievementsFiltered$: Observable<IAchievement[]>;

  constructor(private achievementsService: AchievementsService) {
    this.filterForm = new FormGroup({
      filter: new FormControl()
    });

    this.achievements$ = this.achievementsService.getList();

    this.filter$ = this.filterForm.get('filter').valueChanges.pipe(
      startWith('')
    );

    this.achievementsFiltered$ = combineLatest([this.achievements$, this.filter$]).pipe(
      map(([achievements, filter]) => {
        return this.filter(achievements, filter);
      })
    )
  }

  filter(achievements: IAchievement[], filter: string): IAchievement[] {
    return achievements.filter(achievement => {
      return achievement.name.toLowerCase().includes(filter) ||
        achievement.game.name.toLowerCase().includes(filter) ||
        achievement.description.toLowerCase().includes(filter);
    })
  }

}

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
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