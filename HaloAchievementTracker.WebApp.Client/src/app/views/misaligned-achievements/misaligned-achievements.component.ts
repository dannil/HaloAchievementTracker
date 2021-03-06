import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { AchievementsService } from '@app/services/achievements.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { AngularMaterialModule } from '@app/angular-material.module';
import { LoadingSpinnerModule } from '@app/components/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-misaligned-achievements-component',
  templateUrl: './misaligned-achievements.component.html'
})
export class MisalignedAchievementsComponent {

  misalignedAchievementsForm: FormGroup;

  misalignedAchievements$: Observable<MisalignedAchievement[]>;

  constructor(private achievementsService: AchievementsService) {
    this.misalignedAchievementsForm = new FormGroup({
      xboxLiveGamertagForm: new FormControl(),
      steamId64Form: new FormControl()
    });
    this.misalignedAchievements$ = of([]);
  }

  get xboxLiveGamertag(): string {
    return this.misalignedAchievementsForm.get('xboxLiveGamertagForm').value;
  }

  get steamId64(): string {
    return this.misalignedAchievementsForm.get('steamId64Form').value;
  }

  onSubmit(): void {
    this.misalignedAchievements$ = this.achievementsService.getMisaligned(this.xboxLiveGamertag, this.steamId64);
  }

}

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    LoadingSpinnerModule
  ],
  declarations: [
    MisalignedAchievementsComponent
  ],
  exports: [
    MisalignedAchievementsComponent
  ]
})
export class MisalignedAchievementsModule { }