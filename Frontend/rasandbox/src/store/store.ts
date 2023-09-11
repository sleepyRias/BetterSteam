/* eslint-disable @typescript-eslint/no-explicit-any */
interface State {
  theme: string;
  isAuthenticated: boolean;
  token: string;
}

import AuthService from "../../shared/api/AuthService";

export const storeOptions = {
  state: {
    theme: "light-theme",
    isAuthenticated: false,
    token: "",
  },
  mutations: {
    setTheme(state: State, theme: string) {
      state.theme = theme;
    },
    setAuthentication(state: State, authentication: boolean) {
      state.isAuthenticated = authentication;
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
    setAuthentication(context: any, authentication: boolean) {
      context.commit("setAuthentication", authentication);
    },
    async checkAuthentication(context: any) {
      try {
        // ... (some asynchronous operation, e.g., making an API request)
        const isAuthenticated = await AuthService.checkAuthentication();

        // Committing a mutation to update the state
        context.commit("setAuthentication", isAuthenticated);
      } catch (error) {
        console.error("Authentication check failed:", error);
      }
    },
  },
  getters: {
    getTheme(state: State) {
      return state.theme;
    },
    isAuthenticated: (state: State) => state.isAuthenticated,
  },
};
