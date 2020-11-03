interface MisalignedAchievement {
  name: string;
  game: Game;
  description: string;
  isUnlockedOnSteam: boolean;
  isUnlockedOnXboxLive: boolean;
}
  
interface Game {
  name: string;
}