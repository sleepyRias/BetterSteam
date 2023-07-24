import { GameFilter } from "shared/interfaces/filters";
import { Game } from "../interfaces/Game";

/* eslint-disable @typescript-eslint/no-explicit-any */
export interface SteamRepository {
  getGames(filter: GameFilter): Promise<Game[]>;
}
