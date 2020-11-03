import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from '@app/app.component';
import { NavMenuComponent } from '@components/nav-menu/nav-menu.component';
import { HomeComponent } from '@views/home/home.component';
import { MisalignedAchievementsComponent } from '@app/views/misaligned-achievements/misaligned-achievements.component';
import { AngularMaterialModule } from '@app/angular-material.module';
import { ROUTE_CONFIG } from '@app/route-config';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    MisalignedAchievementsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    RouterModule.forRoot(ROUTE_CONFIG)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
