import { HomeComponent } from "@views/home/home.component";
import { MisalignedAchievementsComponent } from "@app/views/misaligned-achievements/misaligned-achievements.component";

export const ROUTE_CONFIG = [
    {
        path: '', component: HomeComponent
    },
    {
        path: 'misaligned-achievements', component: MisalignedAchievementsComponent
    }
]