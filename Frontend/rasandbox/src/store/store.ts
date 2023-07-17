interface State {
  theme: string;
}

export const storeOptions = {
  state: {
    theme: "light-theme",
  },
  mutations: {
    setTheme(state: State, theme: string) {
      state.theme = theme;
    },
  },
  actions: {
    setTheme(context: any, theme: string) {
      context.commit("setTheme", theme);
    },
  },
  getters: {
    getTheme(state: State) {
      return state.theme;
    },
  },
};
