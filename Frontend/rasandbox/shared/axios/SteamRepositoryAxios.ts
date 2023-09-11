import { AxiosRepository } from "./AxiosRepository";
import { BetterSteamResponse, Token } from "../interfaces/BetterSteamResponse";
import { SteamRepository } from "../repos/SteamRepository";
import { GameFilter } from "shared/interfaces/filters";

export class SteamRepositoryAxios
  extends AxiosRepository
  implements SteamRepository
{
  private basePath = "https://localhost:7091/api";
  public getGames(filter: GameFilter) {
    return this.sendGet<BetterSteamResponse>(
      `${this.basePath}/GameController/Games`,
      {
        params: {
          page: filter.page,
          filter: filter.name,
          gFilter: filter.genre,
          companyFilter: filter.company,
          priceMinFilter: filter.minPrice,
          priceMaxFilter: filter.maxPrice,
          minRDFilter: filter.releaseDate,
          pageSize: filter.pageSize,
        },
      }
    );
  }
  public login(username: string, password: string) {
    const query = {
      username: username,
      password: password,
    };
    return this.sendPost<Token>(
      `${this.basePath}/AccountController/Login`,
      query
    );
  }
}
