import { axios, Cookies } from "../src/components";
import { SteamRepositoryAxios } from "./axios/SteamRepositoryAxios";

const repo = new SteamRepositoryAxios(axios);

export default async function CookieValidation(): Promise<boolean> {
  const token = Cookies.get("token");
  if (token === undefined) {
    return false;
  } else {
    const response = await repo.verify(token);
    return response.isValid;
  }
}
