<template>
  <div :class="themeClass">
    <user-modal v-if="showModal" class="is-active" @close="showModal = false" />
    <filter-modal
      v-if="showFilter"
      class="is-active"
      @close="showFilter = false"
      @submit="updateFilter"
    />
    <div class="main-header">
      <h1 class="main-title">Sandbox Project</h1>
      <button @click="showModal = true" class="button is-link">User</button>
    </div>
    <div class="columns">
      <div class="column">
        <button class="button is-warning" @click="showFilter = !showFilter">
          Filter
        </button>
      </div>
      <div class="colum">
        <button class="button" @click="getGames(10)">G A M E S</button>
      </div>
    </div>
    <div class="columns is-gapless is-multiline">
      <div
        class="column is-one-third"
        v-for="game in filteredList"
        :key="game.name"
      >
        <div
          class="gameBox"
          v-if="
            !filter ||
            (filter.company === game.company &&
              filter.releaseDate === game.releaseDate &&
              isInPricerange(game.price))
          "
        >
          <!-- computed list FILTERN nicht EDITIEREN immer filter list benutzt -->
          <ul>
            <li>{{ game.name }}</li>
            <li>{{ game.price }}€</li>
            <li>{{ game.company }}</li>
            <li>{{ game.releaseDate }}</li>
          </ul>
          <button class="favButton button is-warning">Favorite</button>
          <!-- stern oben rechts font awesome -->
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
const repo = new SteamRepositoryAxios(axios);
import Vue from "vue";
import axios from "axios";
import { Game } from "../shared/interfaces/Game";
import { SteamRepositoryAxios } from "../shared/axios/SteamRepositoryAxios";
import UserModal from "./components/User-modal.vue";
import FilterModal from "./components/Filter-modal.vue";
import { GameFilter } from "../shared/interfaces/filters";
export default Vue.extend({
  name: "App",
  components: {
    UserModal,
    FilterModal,
  },
  data() {
    return {
      gamesList: [] as Game[],
      showModal: false,
      showFilter: false,
      filter: null as GameFilter | null,
    };
  },
  methods: {
    requestGames() {
      this.gamesList = repo.loadGames();
    },
    // WEG
    updateFilter(filter: GameFilter) {
      this.filter = { ...this.filter, ...filter };
    },
    isInPricerange(price: number): boolean {
      if (!this.filter) {
        return true;
      }
      return price >= this.filter.minPrice && price <= this.filter.maxPrice;
    },
    isInGenre(genres: string[]): boolean {
      if (!this.filter) {
        return true;
      }
      return genres.includes(this.filter.genre);
    },
    async getGames(amount: number) {
      this.gamesList = await repo.getGames(amount);
    },
    toggleTheme() {
      const newTheme =
        this.$store.getters.getTheme === "light-theme"
          ? "dark-theme"
          : "light-theme";
      this.$store.dispatch("setTheme", newTheme);
    },
    filterList(element: Game, index: number, array: Game[]) {
      return true;
      // TODO filter im backend niklas hat das was safe
    },
  },
  computed: {
    themeClass() {
      const theme = this.$store.getters.getTheme;
      switch (theme) {
        case "light-theme":
          return "light-theme";
        case "dark-theme":
          return "dark-theme";
        case "red-gradient-theme":
          return "red-gradient-theme";
        default:
          return "light-theme";
      }
    },
    filteredList(): Game[] {
      const list = this.gamesList;
      return list.filter(this.filterList);
    },
  },
  beforeMount() {
    this.requestGames();
  },
});
</script>

<style lang="scss">
@import "../shared/themes.scss";
.main-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin: 5px 5px 60px 5px;
}
.main-title {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  font-size: larger;
  font-weight: 600;
}
.gameBox {
  border: 1px solid black;
  margin: 10px 5px 0px 5px;
  position: relative;
}
.favButton {
  margin-left: 20px;
  position: absolute !important;
  top: 0;
  right: 0;
}
.light-theme {
  background-color: $background-light-theme-color;
  color: $primary-light-theme-color;
}
.dark-theme {
  background-color: $background-dark-theme-color;
  color: $primary-dark-theme-color;
}
.red-gradient-theme {
  background: $background-red-gradient-color;
  color: $primary-red-gradient-color;
}
</style>
