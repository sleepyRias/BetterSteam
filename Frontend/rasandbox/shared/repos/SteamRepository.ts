import { Game } from "../interfaces/Game";

/* eslint-disable @typescript-eslint/no-explicit-any */
export interface SteamRepository {
  loadGames(): any;
  getGames(amount: number): Promise<Game[]>;
}
