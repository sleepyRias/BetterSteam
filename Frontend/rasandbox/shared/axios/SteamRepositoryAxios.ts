import { AxiosRepository } from "./AxiosRepository";
import { Game } from "../interfaces/Game";
import { SteamRepository } from "../repos/SteamRepository";

export class SteamRepositoryAxios
  extends AxiosRepository
  implements SteamRepository
{
  private basePath = "https://localhost:7091/api";
  public getGames(
    page?: number,
    genreFilter?: string,
    company?: string,
    minPrice?: number,
    maxPrice?: number,
    name?: string,
    minRealseDate?: string
  ) {
    return this.sendGet<Game[]>(`${this.basePath}/GameController/Games`, {
      params: {
        page: page,
        filter: name,
        gFilter: genreFilter,
        companyFilter: company,
        priceMinFilter: minPrice,
        priceMaxFilter: maxPrice,
        minRDFilter: minRealseDate,
      },
    });
  }
}
