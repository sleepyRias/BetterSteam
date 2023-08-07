<template>
  <modal-bert @close="$emit('close')">
    <template #main-content>
      <div class="columns is-multiline">
        <div v-if="name !== ''">
          <div class="column">
            <figure class="image is-128x128">
              <img src="https://bulma.io/images/placeholders/128x128.png" />
            </figure>
          </div>
          <div class="column">
            <span>Username: {{ username }}</span>
          </div>
          <div class="column">
            <span>Name: {{ name }}</span>
          </div>
          <div class="column">
            <ul>
              Favorite Games:
              <li v-for="game in favoriteGamesList" :key="game.name">
                {{ game.name }}
              </li>
            </ul>
            <select class="column select" v-model="colorScheme">
              <option :value="'dark-theme'">Darkmode</option>
              <option :value="'light-theme'">Lightmode</option>
              <option :value="'red-gradient-theme'">Red Gradient</option>
            </select>
            <button class="button is-info" @click="saveUserWithKey(username)">
              Save
            </button>
          </div>
        </div>
        <div v-else-if="name === ''">
          <div class="column">
            <span>Loaduser:</span>
            <input v-model="username" class="input" />
            <button
              class="button is-primary"
              @click="loadUserWithKey(username)"
            >
              Load
            </button>
          </div>
          <div class="column">
            <h1>New User</h1>
            <span>Username: </span>
            <input class="input" v-model="newUser.username" />
            <span>Name: </span>
            <input class="input" v-model="newUser.name" />
            <button class="button is-info" @click="createNewUser">
              Create!
            </button>
          </div>
        </div>
      </div>
    </template>
  </modal-bert>
</template>
<script lang="ts">
import Vue from "vue";
import { Game } from "../../shared/interfaces/Game";
import { User } from "../../shared/interfaces/User";
import modalBert from "./modalBert.vue";

export default Vue.extend({
  name: "UserModal",
  components: {
    modalBert,
  },
  data() {
    return {
      username: "",
      name: "",
      favoriteGamesList: [] as Game[],
      colorScheme: "light-theme",
      newUser: {
        name: "",
        username: "",
        favoriteGamesList: [] as Game[],
      } as User,
    };
  },
  methods: {
    createNewUser() {
      const user = {
        name: this.newUser.name,
        username: this.newUser.username,
        favoriteGamesList: this.newUser.favoriteGamesList,
        colorScheme: this.colorScheme,
      };
      localStorage.setItem(user.username, JSON.stringify(user));
      this.loadUserWithKey(user.username);
    },
    loadUserWithKey(key: string) {
      var user = {} as User;
      // eslint-disable-next-line @typescript-eslint/ban-ts-comment
      // @ts-ignore
      user = JSON.parse(localStorage.getItem(key));
      this.name = user.name;
      this.username = user.username;
      this.favoriteGamesList = user.favoriteGamesList;
      this.colorScheme = user.colorScheme;
      this.$store.dispatch("setTheme", this.colorScheme);
    },
    setTheme() {
      this.$store.dispatch("setTheme", this.colorScheme);
    },
    saveUserWithKey(key: string) {
      const user: User = {
        id: 0,
        name: this.name,
        username: key,
        favoriteGamesList: this.favoriteGamesList,
        colorScheme: this.colorScheme,
      };
      localStorage.setItem(user.username, JSON.stringify(user));
      this.setTheme();
    },
  },
  computed: {
    themeClass() {
      return this.$store.getters.getTheme;
    },
  },
});
</script>
