import { GameFilter } from "shared/interfaces/filters";
import {
  BetterSteamResponse,
  Token,
  isValid,
} from "../interfaces/BetterSteamResponse";

/* eslint-disable @typescript-eslint/no-explicit-any */
export interface SteamRepository {
  getGames(filter: GameFilter): Promise<BetterSteamResponse>;
  login(username: string, password: string): Promise<Token>;
  CreateNewUser(username: string, password: string): Promise<string>;
  verify(token: string): Promise<isValid>;
  CheckUserNameAvailability(username: string): Promise<boolean>;
}
