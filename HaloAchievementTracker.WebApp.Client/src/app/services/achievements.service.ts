import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EnvironmentService } from '@app/services/environment.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AchievementsService {

  private controllerRoute: string;

  constructor(private http: HttpClient, private environmentService: EnvironmentService) {
    this.controllerRoute = `${this.environmentService.apiUrl}/achievements`
  }

  getList(): Observable<IAchievement[]> {
    const endpoint = `${this.controllerRoute}/list`;

    const response = this.http.get<IAchievement[]>(endpoint);
    return response;
  }

  getMisaligned(xboxLiveGamertag: string, steamId64: string): Observable<MisalignedAchievement[]> {
    const endpoint = `${this.controllerRoute}/misaligned`;
    const params = new HttpParams()
      .set('xboxLiveGamertag', xboxLiveGamertag)
      .set('steamId64', steamId64);

    const response = this.http.get<MisalignedAchievement[]>(endpoint, { params: params });
    return response;
  }

}
