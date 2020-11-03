import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { MisalignedAchievementsService } from '@services/misalignedachievements.service';

@Component({
  selector: 'app-misaligned-achievements-component',
  templateUrl: './misaligned-achievements.component.html'
})
export class MisalignedAchievementsComponent {

  misalignedAchievementsForm: FormGroup;

  misalignedAchievements$: Observable<MisalignedAchievement[]>;

  constructor(private misalignedAchievementsService: MisalignedAchievementsService) {
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
