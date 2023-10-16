import { Game } from "../../src/components";
export interface BetterSteamResponse {
  games: Game[];
  totalCount: number;
}
export interface Token {
  token: string;
}
export interface isValid {
  isValid: boolean;
}
export interface WishlistResponse {
  accountId: number;
  gameId: number;
  id: number;
}
