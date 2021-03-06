import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from '@app/app.component';
import { ROUTE_CONFIG } from '@app/route-config';
import { NavMenuModule } from '@app/components/nav-menu/nav-menu.component';
import { MisalignedAchievementsModule } from '@app/views/misaligned-achievements/misaligned-achievements.component';
import { HomeModule } from '@app/views/home/home.component';
import { ListAchievementsModule } from '@app/views/list-achievements/list-achievements.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    NavMenuModule,
    HomeModule,
    ListAchievementsModule,
    MisalignedAchievementsModule,
    RouterModule.forRoot(ROUTE_CONFIG, {
      relativeLinkResolution: 'legacy' 
    })
  ],
  providers: [],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
