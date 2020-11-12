import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EnvironmentService } from '@app/services/environment.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertService } from './alert.service';

@Injectable({
  providedIn: 'root'
})
export class AchievementsService {

  private controllerRoute: string;

  constructor(private http: HttpClient, private environment: EnvironmentService, private alertService: AlertService) {
    this.controllerRoute = `${this.environment.apiUrl}/achievements`
  }

  getMisaligned(xboxLiveGamertag: string, steamId64: string): Observable<MisalignedAchievement[]> {
    const endpoint = `${this.controllerRoute}/misaligned`;
    const params = new HttpParams()
      .set('xboxLiveGamertag', xboxLiveGamertag)
      .set('steamId64', steamId64);

    const response = this.http.get<MisalignedAchievement[]>(endpoint, { params: params })
      .pipe(
        catchError(error => {
          this.alertService.error(error.message);
          return of([]);
        })
      );

    return response;
  }

}
