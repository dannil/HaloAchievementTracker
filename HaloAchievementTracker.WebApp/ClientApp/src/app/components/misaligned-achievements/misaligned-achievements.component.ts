import { Component } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { EnvironmentService } from '@services/environment.service';
import { MisalignedAchievementsService } from '@services/misalignedachievements.service';

@Component({
  selector: 'app-misaligned-achievements-component',
  templateUrl: './misaligned-achievements.component.html'
})
export class MisalignedAchievementsComponent {

  private misalignedAchievementsForm: FormGroup;

  private misalignedAchievements$: Observable<MisalignedAchievement[]>;

  constructor(private http: HttpClient, private environment: EnvironmentService, private misalignedAchievementsService: MisalignedAchievementsService) {
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
    this.misalignedAchievements$ = this.misalignedAchievementsService.getQuery(this.xboxLiveGamertag, this.steamId64);
  }

}
