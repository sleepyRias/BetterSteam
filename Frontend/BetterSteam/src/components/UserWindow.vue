<template>
  <div class="columns">
    <div class="column is-3">
      <div class="user-controls">
        <figure class="image is-128x128">
          <img src="https://bulma.io/images/placeholders/128x128.png" />
        </figure>
        <div>Username: {{ username }}</div>
        <div>Name: {{ name }}</div>
      </div>
    </div>
    <div class="column is-1"></div>
    <div class="column is-3">
      <span>Favorite Games List:</span>
      <game-box
        :Game="{
          id: 1,
          name: 'Test',
          company: 'Test',
          price: 69.42,
          releaseDate: '18-09-2000',
          genre: 1,
        }"
        :enableHover="false"
      />
    </div>
    <div class="column is-1"></div>
    <div class="column is-3">
      <span>Games on Wishlist:</span>
      <game-box
        :Game="{
          id: 1,
          name: 'Test',
          company: 'Test',
          price: 69.42,
          releaseDate: '30-01-2222',
          genre: 1,
        }"
        :enableHover="false"
      />
    </div>
    <div class="column is-auto"></div>
  </div>
</template>
<script lang="ts">
import { Vue, GameBox, axios, Cookies } from "./";
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
const repo = new SteamRepositoryAxios(axios);
export default Vue.extend({
  name: "SuperSecret",
  components: {
    GameBox,
  },
  data() {
    return {
      username: "test",
      name: "Test",
      isAllowedUsernameChange: false,
    };
  },
  methods: {
    async verify() {
      const token = Cookies.get("token");
      if (token === undefined) {
        return false;
      }
      const response = await repo.verify(token);
      if (response.isValid) {
        return true;
      }
      return false;
    },
  },
  beforeMount() {
    if (!this.verify()) {
      this.$router.push("/login");
    }
  },
});
</script>
<style lang="scss" scoped>
.user-controls {
  margin: 10px 0 0 10px;
}
</style>
