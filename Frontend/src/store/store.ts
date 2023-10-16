/* eslint-disable @typescript-eslint/no-explicit-any */
import { WishlistResponse, axios, Cookies } from "../components";
import { SteamRepositoryAxios } from "../../public/axios/SteamRepositoryAxios";
const api = new SteamRepositoryAxios(axios);
interface State {
  theme: string;
  wishlist: WishlistResponse[];
  token: string;
}

export const storeOptions = {
  state: {
    theme: "light-theme",
    wishlist: [],
    token: "",
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
    async fetchWishlist(context: any) {
      const token = Cookies.get("token");
      if (token === undefined) return;
      const response = await api.getWishlist(token);
      context.commit("setWishlist", response);
    },
    setToken(context: any, token: string) {
      context.commit("setToken", token);
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
