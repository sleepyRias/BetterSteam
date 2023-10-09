import {
  BetterSteamResponse,
  Token,
  isValid,
  SteamRepository,
  GameFilter,
  User,
} from "../../src/components";
import { AxiosRepository } from "./AxiosRepository";

export class SteamRepositoryAxios
  extends AxiosRepository
  implements SteamRepository
{
  private basePath = "https://localhost:7091/api";
  public getGames(filter: GameFilter) {
    return this.sendGet<BetterSteamResponse>(`${this.basePath}/Game/Games`, {
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
    });
  }
  public login(username: string, password: string) {
    const query = {
      username: username,
      password: password,
    };
    return this.sendPost<Token>(`${this.basePath}/Account/Login`, query);
  }
  public verify(token: string) {
    return this.sendPost<isValid>(`${this.basePath}/Account/Verify`, {
      token: token,
    });
  }
  public checkUserNameAvailability(username: string) {
    return this.sendGet<boolean>(
      `${this.basePath}/Account/CheckUserNameAvailability`,
      {
        params: {
          username: username,
        },
      }
    );
  }
  public createNewUser(username: string, password: string) {
    const query = {
      username: username,
      password: password,
    };
    return this.sendPost<string>(
      `${this.basePath}/Account/CreateAccount`,
      query
    );
  }
  public GetRandomPreview(): Promise<string> {
    return this.sendGet<string>(`${this.basePath}/Image/GetRandomPreview`);
  }
  public addFavoriteGame(token: string, gameId: string): Promise<string> {
    const query = {
      token: token,
      value1: gameId,
      // @Robert das hat niklas so gemacht und da must du mit ihm reden ich werde dafür net bezahlt
    };
    return this.sendPost<string>(
      `${this.basePath}/Account/AddFavouriteGame`,
      query
    );
  }
  public getNameFromToken(token: string): Promise<User> {
    const query = {
      token: token,
    };
    return this.sendPost<User>(
      `${this.basePath}/Account/GetNameFromToken`,
      query
    );
  }
  public removeFavouriteGame(token: string, gameId: string): Promise<string> {
    const query = {
      token: token,
      value1: gameId,
    };
    return this.sendDelete<string>(
      `${this.basePath}/Account/RemoveFavouriteGame`,
      query
    );
  }
}
