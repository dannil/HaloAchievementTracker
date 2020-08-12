import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-misaligned-achievements-component',
  templateUrl: './misaligned-achievements.component.html'
})
export class MisalignedAchievementsComponent {

  private readonly XBOX_LIVE_GAMERTAG_QUERY_PARAM = 'xboxLiveGamertag';
  private readonly STEAM_ID_QUERY_PARAM = 'steamId64';

  private misalignedAchievementsForm: FormGroup;

  private misalignedAchievements$: Observable<MisalignedAchievement[]>;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute, private router: Router) {
    const url = `${baseUrl}api/misalignedachievements`;

    const xboxLiveGamertagParam = route.snapshot.queryParamMap.get(this.XBOX_LIVE_GAMERTAG_QUERY_PARAM);
    const steamId64Param = route.snapshot.queryParamMap.get(this.STEAM_ID_QUERY_PARAM);

    this.misalignedAchievementsForm = new FormGroup({
      xboxLiveGamertagForm: new FormControl(xboxLiveGamertagParam),
      steamId64Form: new FormControl(steamId64Param)
    });

    if (xboxLiveGamertagParam && steamId64Param) {
      const params = new HttpParams()
        .set('xboxLiveGamerTag', xboxLiveGamertagParam)
        .set('steamId64', steamId64Param);

      this.misalignedAchievements$ = http.get<MisalignedAchievement[]>(url, { params: params });
    } else {
      this.misalignedAchievements$ = of([]);
    }
  }

  get xboxLiveGamertag(): string {
    return this.misalignedAchievementsForm.get('xboxLiveGamertagForm').value;
  }

  get steamId64(): string {
    return this.misalignedAchievementsForm.get('steamId64Form').value;
  }

  onSubmit(): void {
    this.router.navigate(['/misaligned-achievements'], {
      relativeTo: this.route,
      queryParams: {
        xboxLiveGamertag: this.xboxLiveGamertag,
        steamId64: this.steamId64
      }
    });
  }

}

interface MisalignedAchievement {
  name: string;
  gameId: string;
  description: string;
}
