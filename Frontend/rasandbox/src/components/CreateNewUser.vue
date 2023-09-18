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
      <button class="button is-success">Create User</button>
    </div>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  name: "CreateNewUser",
  data() {
    return {
      username: "",
      password: "",
      password2: "",
    };
  },
  methods: {
    backToHome() {
      this.$router.push("/games");
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
</style>
