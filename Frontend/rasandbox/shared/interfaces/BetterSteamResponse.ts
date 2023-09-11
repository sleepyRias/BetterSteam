import { Game } from "./Game";
export interface BetterSteamResponse {
  games: Game[];
  totalCount: number;
}
export interface Token {
  token: string;
}
