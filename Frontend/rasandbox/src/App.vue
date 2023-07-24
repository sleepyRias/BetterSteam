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
    <h3>show {{ filter?.page }} items per page</h3>
    <div class="buttons has-addons">
      <button class="button" @click="setPageAmountTo(20)">20 items</button>
      <button class="button" @click="setPageAmountTo(40)">40 items</button>
      <button class="button" @click="setPageAmountTo(60)">60 items</button>
    </div>
    <i
      v-if="isGamesLoading"
      class="fa-solid fa-spinner fa-4x"
      :class="{ 'loading-spinner': isGamesLoading }"
    />
    <div v-if="!isGamesLoading" class="columns is-gapless is-multiline">
      <div
        class="column is-one-third"
        v-for="game in filteredList"
        :key="game.id"
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
      isGamesLoading: false,
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
    async getGames() {
      this.gamesList = await repo.getGames();
    },
    setPageAmountTo(amount: number) {
      if (this.filter) {
        this.filter.page = amount;
      }
    },
    filterList() {
      if (this.filter) {
        repo.getGames(
          this.filter.page,
          this.filter.genre,
          this.filter.company,
          this.filter.minPrice,
          this.filter.maxPrice,
          this.filter.name,
          this.filter.releaseDate
        );
      }
      return true;
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
    this.isGamesLoading = true;
    this.getGames();
    this.isGamesLoading = false;
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
.loading-spinner {
  animation: spin 2s linear infinite; // animation name duration speed and repeating
  position: absolute;
  top: 50%;
  right: 50%;
}
@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
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
