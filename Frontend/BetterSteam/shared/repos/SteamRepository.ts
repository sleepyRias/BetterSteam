import { User, GameFilter, Token } from "../../src/components";
import {
  BetterSteamResponse,
  isValid,
} from "../interfaces/BetterSteamResponse";

/* eslint-disable @typescript-eslint/no-explicit-any */
export interface SteamRepository {
  getGames(filter: GameFilter): Promise<BetterSteamResponse>;
  login(username: string, password: string): Promise<Token>;
  createNewUser(username: string, password: string): Promise<string>;
  verify(token: string): Promise<isValid>;
  checkUserNameAvailability(username: string): Promise<boolean>;
  addFavoriteGame(token: string, gameId: string): Promise<string>;
  removeFavouriteGame(token: string, gameId: string): Promise<string>;
  getNameFromToken(token: string): Promise<User>;
  addToWishlist(token: string, gameId: string): Promise<string>;
  removeFromWishlist(token: string, gameId: string): Promise<string>;
}
