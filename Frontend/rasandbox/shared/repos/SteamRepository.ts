import { Game } from "../interfaces/Game";

/* eslint-disable @typescript-eslint/no-explicit-any */
export interface SteamRepository {
  getGames(
    page?: number,
    genreFilter?: string,
    company?: string,
    minPrice?: number,
    maxPrice?: number,
    name?: string,
    minRealseDate?: string
  ): Promise<Game[]>;
}
