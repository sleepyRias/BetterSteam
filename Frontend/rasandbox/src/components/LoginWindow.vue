<template>
  <div>
    <form @submit="login">
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
      <button type="submit" class="button">Login</button>
    </form>
  </div>
</template>
<script lang="ts">
import Vue from "vue";

export default Vue.extend({
  name: "LoginWindow",
  data() {
    return {
      username: "",
      password: "",
    };
  },
  methods: {
    login(event: any) {
      event.preventDefault();
      // Send a login request to your server and handle the response.
      // If authentication is successful, store a token or session on the client side.
      // You can use Vuex, local storage, or cookies to store this information.
    },
  },
});
</script>
