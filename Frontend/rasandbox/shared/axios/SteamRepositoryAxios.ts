import { AxiosRepository } from "./AxiosRepository";
import { BetterSteamResponse } from "../interfaces/BetterSteamResponse";
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
}
