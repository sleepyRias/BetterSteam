<template>
  <div>
    <button class="button back-home" @click="backToHome">
      <span class="icon">
        <i class="fa-solid fa-arrow-left" />
      </span>
      <span>Back to Home</span>
    </button>
    <div class="CreateUser">
      <input
        v-model="username"
        type="text"
        placeholder="Username"
        class="input"
      />
      <span v-if="username !== ''" :class="usernameAvailable">{{
        usernameAvailability
          ? "Username is Available"
          : "Username is NOT Available"
      }}</span>
      <input
        v-model="password"
        type="password"
        placeholder="Password"
        class="input"
      />
      <input
        v-model="password2"
        type="password"
        placeholder="Repeat Password"
        class="input"
      />
      <div class="button-group">
        <button type="submit" class="button is-success" @click="CreateNewUser">
          Create User
        </button>
        <span class="login-Account" @click="goToLogin"
          >Already have an Account? Click here to Login</span
        >
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
import axios from "axios";
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
const repo = new SteamRepositoryAxios(axios);
export default Vue.extend({
  name: "CreateNewUser",
  data() {
    return {
      username: "",
      password: "",
      password2: "",
      usernameAvailability: false,
    };
  },
  methods: {
    backToHome() {
      this.$router.push("/games");
    },
    async checkUsernameAvailability(username: string) {
      var response = false;
      try {
        response = await repo.CheckUserNameAvailability(username);
      } catch (error) {
        this.usernameAvailability = false;
        return false;
      }
      this.usernameAvailability = response;
    },
    async CreateNewUser() {
      if (this.password !== this.password2) {
        alert("Passwords do not match");
        return;
      }
      if (
        this.username === "" ||
        this.password === "" ||
        this.password2 === ""
      ) {
        alert("Fields need to be filled out");
        return;
      }
      if (!this.usernameAvailability) {
        alert("this Username has been taken");
        return;
      }
      const response = await repo.CreateNewUser(this.username, this.password);
      console.log(response);
      this.$router.push("/login");
    },
    goToLogin() {
      this.$router.push("/login");
    },
  },
  computed: {
    usernameAvailable(): string {
      if (this.usernameAvailability) {
        return "UsernameAvailable";
      }
      return "UsernameNotAvailable";
    },
  },
  watch: {
    username: {
      deep: true,
      immediate: false,
      handler() {
        this.checkUsernameAvailability(this.username);
      },
    },
  },
});
</script>
<style lang="scss" scoped>
.CreateUser {
  width: 40%;
  margin: 100px auto 0 auto;
  position: relative;
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
.UsernameNotAvailable {
  color: darkred;
}
.UsernameAvailable {
  color: darkgreen;
}
.button-group {
  display: flex;
  justify-content: space-between;
}
.login-Account {
  text-decoration: underline;
  cursor: pointer;
  font-size: 10pt;
}
</style>
