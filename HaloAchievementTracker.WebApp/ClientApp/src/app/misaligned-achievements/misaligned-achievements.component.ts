import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { MatProgressSpinner } from '@angular/material/progress-spinner'
import { Observable } from 'rxjs';

@Component({
  selector: 'app-misaligned-achievements-component',
  templateUrl: './misaligned-achievements.component.html'
})
export class MisalignedAchievementsComponent {

  private misalignedAchievementsForm: FormGroup;
  private xboxLiveGamerTagForm: FormControl;
  private steamId64Form: FormControl;

  private misalignedAchievements$: Observable<MisalignedAchievement[]>;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, route: ActivatedRoute) {
    const url = `${baseUrl}api/misalignedachievements`;

    const xboxLiveGamerTagParam = route.snapshot.queryParamMap.get('xboxLiveGamertag');
    const steamId64Param = route.snapshot.queryParamMap.get('steamId64');

    this.misalignedAchievementsForm = new FormGroup({
      xboxLiveGamerTagForm: new FormControl(xboxLiveGamerTagParam),
      steamId64Form: new FormControl(steamId64Param)
    });

    const params = new HttpParams()
      .set('xboxLiveGamerTag', xboxLiveGamerTagParam)
      .set('steamId64', steamId64Param);

    this.misalignedAchievements$ = http.get<MisalignedAchievement[]>(url, { params: params });

    //http.get<MisalignedAchievement[]>(url, { params: params }).subscribe(result => {
    //  //this.misalignedAchievements = result;
    //}, error => console.error(error));
  }

}

interface MisalignedAchievement {
  name: string;
  gameId: string;
  description: string;
}
