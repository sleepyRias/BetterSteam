<template>
  <div>
    <input
      v-model="username"
      type="text"
      placeholder="Username"
      class="input"
    />
    <input
      v-model="password"
      type="password"
      placeholder="Password"
      class="input"
    />
    <button type="submit" class="button" @click="login">Login</button>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import VueCookies from "vue-cookies";
Vue.use(VueCookies);
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
import axios from "axios";
const repo = new SteamRepositoryAxios(axios);
export default Vue.extend({
  name: "LoginWindow",
  data() {
    return {
      username: "",
      password: "",
    };
  },
  methods: {
    async login() {
      try {
        const response = await repo.login(this.username, this.password);
        localStorage.setItem("token", response.token);
        this.$store.dispatch("setToken", response.token);
      } catch (error) {
        alert("Invalid credentials");
      }
      this.verify();
      if (this.$store.state.isAuthenticated === true) {
        this.$cookies.set("token", this.$store.state.token, "7d");
        this.$router.push("/ss");
      }
    },
    async verify() {
      const response = await repo.verify(this.$store.state.token);
      if (response.isValid) {
        this.$store.state.isAuthenticated = true;
      }
    },
  },
});
</script>
