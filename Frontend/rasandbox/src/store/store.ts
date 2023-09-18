/* eslint-disable @typescript-eslint/no-explicit-any */

import axios from "axios";
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
const repo = new SteamRepositoryAxios(axios);

interface State {
  theme: string;
  token: string;
}

export const storeOptions = {
  state: {
    theme: "light-theme",
    token: "",
  },
  mutations: {
    setTheme(state: State, theme: string) {
      state.theme = theme;
    },
    setToken(state: State, token: string) {
      state.token = token;
    },
  },
  actions: {
    setTheme(context: any, theme: string) {
      context.commit("setTheme", theme);
    },
    setToken(context: any, token: string) {
      context.commit("setToken", token);
    },
  },
  getters: {
    getTheme(state: State) {
      return state.theme;
    },
  },
};
