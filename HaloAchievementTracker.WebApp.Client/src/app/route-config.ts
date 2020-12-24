import { HomeComponent } from '@app/views/home/home.component';
import { MisalignedAchievementsComponent } from '@app/views/misaligned-achievements/misaligned-achievements.component';
import { ListAchievementsComponent } from '@app/views/list-achievements/list-achievements.component';

export const ROUTE_CONFIG = [
    {
        path: '', component: HomeComponent
    },
    {
        path: 'list-achievements', component: ListAchievementsComponent
    },
    {
        path: 'misaligned-achievements', component: MisalignedAchievementsComponent
    }
]