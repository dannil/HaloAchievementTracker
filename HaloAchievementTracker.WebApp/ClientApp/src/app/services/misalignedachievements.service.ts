import { IEnvironment } from '@environments/ienvironment';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EnvironmentService } from './environment.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MisalignedAchievementsService {

  constructor(private http: HttpClient, private environment: EnvironmentService) {
    
  }

  getQuery(xboxLiveGamertag: string, steamId64: string): Observable<MisalignedAchievement[]> {
    const url = `${this.environment.apiUrl}/misalignedachievements`;
    const params = new HttpParams()
      .set('xboxLiveGamertag', xboxLiveGamertag)
      .set('steamId64', steamId64);

    const response = this.http.get<MisalignedAchievement[]>(url, { params: params });
    return response;
  }

}
