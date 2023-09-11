// AuthService.js
import axios from "axios";
import { storeOptions } from "../../src/store/store";

export default {
  async login(username: string, password: string) {
    try {
      const response = await axios.post("/api/login", { username, password });
      const token = response.data.token; // Assuming the server responds with a token
      // Store the token in local storage or Vuex store
      localStorage.setItem("token", token);
      storeOptions.actions.setToken("setToken", token);
    } catch (error) {
      console.error("Login failed:", error);
    }
  },
  async checkAuthentication() {
    return;
  },
  // Other authentication-related functions...
};
