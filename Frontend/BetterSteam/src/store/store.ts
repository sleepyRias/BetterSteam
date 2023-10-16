/* eslint-disable @typescript-eslint/no-explicit-any */
import { WishlistResponse, axios, Cookies } from "@/components";
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
const api = new SteamRepositoryAxios(axios);
interface State {
  theme: string;
  wishlist: WishlistResponse[];
  token: string | undefined;
}

export const storeOptions = {
  state: {
    theme: "light-theme",
    wishlist: [],
    token: undefined,
  },
  mutations: {
    setTheme(state: State, theme: string) {
      state.theme = theme;
    },
    setWishlist(state: State, wishlist: WishlistResponse[]) {
      state.wishlist = wishlist;
    },
    setToken(state: State, token: string) {
      state.token = token;
    },
  },
  actions: {
    setTheme(context: any, theme: string) {
      context.commit("setTheme", theme);
    },
    async fetchWishlist(context: any, token: string) {
      const response = await api.getWishlist(token);
      context.commit("setWishlist", response);
    },
    async verifyToken(_: any, token: string): Promise<boolean> {
      try {
        const data = (await api.verify(token)).isValid;
        return data;
      } catch (error) {
        return false;
      }
    },
    setToken(context: any, token: string) {
      context.commit("setToken", token);
    },
    fetchToken(_: any) {
      return Cookies.get("token");
    },
  },
  getters: {
    getTheme(state: State) {
      return state.theme;
    },
    getToken(state: State) {
      return state.token;
    },
    getWishlist(state: State) {
      return state.wishlist;
    },
  },
};
