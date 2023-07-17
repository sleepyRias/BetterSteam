import { Game } from "./Game";

export interface User {
  id: number;
  name: string;
  username: string;
  favoriteGamesList: Game[];
  colorScheme: string;
}
