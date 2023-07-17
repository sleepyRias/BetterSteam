<template>
  <div class="betterSteam" :class="themeClass">
    <user-modal v-if="showUser" class="is-active" @close="showUser = false" />
    <filter-modal
      v-if="showFilter"
      class="is-active"
      @close="showFilter = false"
      @submit="updateFilter"
    />
    <div class="main-header">
      <h1 class="main-title">Sandbox Project</h1>
      <button @click="showUser = true" class="user-button">
        <i class="fa-regular fa-user fa-2x" />
      </button>
    </div>
    <button class="button" @click="showFilter = !showFilter">
      <span class="icon">
        <i class="fa-solid fa-filter" />
      </span>
      <span>Filter</span>
    </button>
    <div class="columns is-gapless is-multiline">
      <div
        class="column is-one-third"
        v-for="game in filteredList"
        :key="game.name"
      >
        <div class="gameBox">
          <ul>
            <li>{{ game.name }}</li>
            <li>{{ game.price }}€</li>
            <li>{{ game.company }}</li>
            <li>{{ game.releaseDate }}</li>
          </ul>
          <button class="favButton" @click="isFavorited = !isFavorited">
            <span class="icon">
              <i
                :class="{ 'fa-regular': !isFavorited, 'fa-solid': isFavorited }"
                class="fa-star fa-lg"
              />
            </span>
          </button>
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
      showUser: false,
      showFilter: false,
      filter: null as GameFilter | null,
      isFavorited: false,
    };
  },
  methods: {
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
    filterList() {
      if (this.filter) {
        repo.filterGames(
          this.filter.genre,
          this.filter.company,
          this.filter.minPrice,
          this.filter.maxPrice,
          this.filter.name,
          this.filter.releaseDate
        );
      }
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
    this.getGames(60);
  },
});
</script>

<style lang="scss">
@import "../shared/themes.scss";
html,
body {
  height: 100vh;
}
.betterSteam {
  height: 100%;
}
.user-button {
  border: none;
  background: none;
  color: #1b1d9e;
  margin: 5px 5px 0 0;
}
.main-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
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
  position: absolute !important;
  top: 0;
  right: 0;
  border: none;
  background: none;
  cursor: pointer;
  padding: 0;
  margin: 4px 6px;
  color: #fcd303;
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
