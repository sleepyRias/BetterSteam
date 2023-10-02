<template>
  <div>
    <button class="button back-home" @click="backToHome">
      <span class="icon">
        <i class="fa-solid fa-arrow-left" />
      </span>
      <span>Back to Home</span>
    </button>
    <div class="main-login-form">
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
      <div class="button-group">
        <button type="submit" class="button is-success" @click="login">
          Login
        </button>
        <span class="create-Account" @click="goToCreateNewUser"
          >Don't have an Account? Create one here</span
        >
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { Vue, Cookies, SteamRepositoryAxios, axios, Token } from "./index";
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
      var response = {} as Token;
      // i shouldn't do this but i am doin this
      try {
        response = await repo.login(this.username, this.password);
      } catch (error) {
        alert("Invalid credentials");
      }
      Cookies.set("token", response.token);
      if (Cookies.get("token") && (await this.verify(response.token))) {
        this.$router.push("/user");
      }
    },
    async verify(token: string): Promise<boolean> {
      const response = await repo.verify(token);
      if (response.isValid) {
        return true;
      }
      return false;
    },
    backToHome() {
      this.$router.push("/games");
    },
    goToCreateNewUser() {
      this.$router.push("/newUser");
    },
  },
});
</script>
<style lang="scss" scoped>
.main-login-form {
  width: 40%;
  margin: 100px auto 0 auto;
  position: relative;
}
.button-group {
  display: flex;
  justify-content: space-between;
}
.create-Account {
  text-decoration: underline;
  cursor: pointer;
  font-size: 10pt;
}
.back-home {
  text-decoration: underline;
  position: absolute;
  top: 0;
  left: 0;
  margin: 15px 0 0 15px;
  border: none;
}
.button:focus:not(:active),
.button.is-focused:not(:active) {
  box-shadow: none !important;
}
</style>
