import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-misaligned-achievements-component',
  templateUrl: './misaligned-achievements.component.html'
})
export class MisalignedAchievementsComponent {

  public misalignedAchievements: MisalignedAchievement[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, route: ActivatedRoute) {
    const url = `${baseUrl}api/misalignedachievements`;

    let params = new HttpParams()
      .set('xboxLiveGamerTag', route.snapshot.queryParamMap.get('xboxLiveGamerTag'))
      .set('steamId64', route.snapshot.queryParamMap.get('steamId64'));

    http.get<MisalignedAchievement[]>(url, { params: params }).subscribe(result => {
      this.misalignedAchievements = result;
    }, error => console.error(error));
  }

}

interface MisalignedAchievement {
  name: string;
  gameId: string;
  description: string;
}
