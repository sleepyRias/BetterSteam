/* eslint-disable @typescript-eslint/no-explicit-any */
interface State {
  theme: string;
  isAuthenticated: boolean;
  token: string;
}

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
    async checkAuthentication() {
      return;
    },
  },
  getters: {
    getTheme(state: State) {
      return state.theme;
    },
    isAuthenticated: (state: State) => state.isAuthenticated,
  },
};
