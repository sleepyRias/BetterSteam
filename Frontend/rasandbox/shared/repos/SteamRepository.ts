import { GameFilter } from "shared/interfaces/filters";
import { BetterSteamResponse } from "../interfaces/BetterSteamResponse";

/* eslint-disable @typescript-eslint/no-explicit-any */
export interface SteamRepository {
  getGames(filter: GameFilter): Promise<BetterSteamResponse>;
  login(username: string, password: string): Promise<string>;
}
