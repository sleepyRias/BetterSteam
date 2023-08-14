<template>
  <div class="betterSteam" :class="themeClass">
    <user-modal v-if="showUser" @close="showUser = false" />
    <filter-modal
      v-if="showFilter"
      @close="showFilter = false"
      @submit="updateFilter"
      @clearFilter="clearFilter"
    />
    <div class="main-header">
      <h1 class="main-title">Sandbox Project</h1>
      <button @click="showUser = true" class="betterSteamButton--user">
        <i class="fa-regular fa-user fa-2x" />
      </button>
    </div>
    <button class="button" @click="showFilter = !showFilter">
      <span class="icon">
        <i class="fa-solid fa-filter" />
      </span>
      <span>Filter</span>
    </button>
    <div class="items-per-page">
      show
      <div class="select-container">
        <select v-model="filter.pageSize" class="selecter">
          <option value="20">20 items</option>
          <option value="40">40 items</option>
          <option value="60">60 items</option>
        </select>
      </div>
      items per page
    </div>

    <div class="field is-grouped">
      <button class="button" @click="prevPage" :disabled="filter.page == 1">
        <span>
          <i class="fa-solid fa-arrow-left"></i>
        </span>
      </button>
      <button class="button is-static">
        {{ filter?.page }}
      </button>
      <button class="button">
        <span>
          <i class="fa-solid fa-arrow-right" @click="nextPage"></i>
        </span>
      </button>
    </div>
    <div>found {{ totalGamesCount }} Games</div>
    <i
      v-if="isGamesLoading"
      class="fa-solid fa-spinner fa-4x loading-spinner"
    />
    <div v-if="!isGamesLoading" class="columns is-gapless is-multiline">
      <div
        class="column is-one-quarter"
        v-for="game in gamesList"
        :key="game.id"
      >
        <game-box :Game="game" />
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
import GameBox from "./components/GameBox.vue";
export default Vue.extend({
  name: "App",
  components: {
    UserModal,
    FilterModal,
    GameBox,
  },
  data() {
    return {
      totalGamesCount: 0,
      gamesList: [] as Game[],
      showUserOLD: false,
      showFilterOLD: false,
      filter: {
        page: 1,
        name: "",
        company: "",
        genre: "",
        minPrice: 0,
        maxPrice: 100,
        releaseDate: "",
        pageSize: 20,
      } as GameFilter,
      isFavorited: false,
      isGamesLoading: false,
    };
  },
  methods: {
    updateFilter(filter: GameFilter) {
      this.filter = { ...this.filter, ...filter };
      this.updateRoute();
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
      const result = await repo.getGames(this.filter);
      this.gamesList = result.games;
      this.totalGamesCount = result.totalCount;
    },
    nextPage() {
      this.filter.page++;
      this.updateRoute();
    },
    prevPage() {
      this.filter.page--;
      this.updateRoute();
    },
    updateRoute() {
      // basically stfu eslint and dont worry i dont know what im doings
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const queryParameters: any = {};

      // Add the parameters to the queryParameters object if they have non-empty values (GPT did this)
      if (this.filter.page !== 0) queryParameters.page = this.filter.page;
      if (this.filter.genre !== "") queryParameters.genre = this.filter.genre;
      if (this.filter.name !== "") queryParameters.name = this.filter.name;
      if (this.filter.company !== "")
        queryParameters.company = this.filter.company;
      if (this.filter.minPrice !== 0)
        queryParameters.minPrice = this.filter.minPrice;
      if (this.filter.maxPrice !== 100)
        queryParameters.maxPrice = this.filter.maxPrice;
      if (this.filter.releaseDate !== "")
        queryParameters.releaseDate = this.filter.releaseDate;

      // Check if the new query parameters are different from the current ones
      const currentQuery = this.$route.query;
      const isDifferent = Object.keys(queryParameters).some((key) => {
        return queryParameters[key] !== currentQuery[key];
      });

      // Perform the navigation only if the query parameters are different
      if (isDifferent) {
        this.$router.push({ path: "/games", query: queryParameters });
      }
    },
    clearFilter() {
      const defaultFilter = {
        page: 1,
        name: "",
        company: "",
        genre: "",
        minPrice: 0,
        maxPrice: 100,
        releaseDate: "",
        pageSize: 20,
      };
      this.filter = { ...defaultFilter };
    },
  },
  computed: {
    themeClass(): string {
      return this.$store.getters.getTheme;
    },
    showUser: {
      get() {
        return this.$route.path === "/user"; // Check if the route is '/user'
      },
      set(value) {
        if (value) {
          this.$router.push("/user"); // Set the route to '/user'
        } else {
          this.$router.push("/games"); // Set the route back to '/games'
        }
      },
    },
    showFilter: {
      get() {
        return this.$route.path === "/filter"; // Check if the route is '/filter'
      },
      set(value) {
        if (value) {
          this.$router.push("/filter"); // Set the route to '/filter'
        } else {
          this.$router.push("/games"); // Set the route back to '/games'
        }
      },
    },
  },
  watch: {
    filter: {
      deep: true,
      immediate: true,
      handler() {
        this.getGames();
      },
    },
  },
  created() {
    this.filter.page = Number(this.$route.query.page || 1);
    this.filter.company = String(this.$route.query.company || "");
    this.filter.genre = String(this.$route.query.genre || "");
    this.filter.name = String(this.$route.query.name || "");
    this.filter.releaseDate = String(this.$route.query.releaseDate || "");
    this.filter.minPrice = Number(this.$route.query.minPrice || 0);
    this.filter.maxPrice = Number(this.$route.query.maxPrice || 100);
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
  padding: 0 20px 0 20px;
}
.betterSteamButton {
  border: none;
  background: none;
  cursor: pointer;
  &--user {
    @extend .betterSteamButton;
    color: #1b1d9e;
    margin: 5px 5px 0 0;
  }
  &--favorite {
    @extend .betterSteamButton;
    position: absolute !important;
    top: 0;
    right: 0;
    padding: 0;
    margin: 4px 6px;
    color: #fcd303;
  }
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
.items-per-page {
  display: flex;
  align-items: center; /* Center vertically */
  margin: 8px 0 8px 0;
  font-size: 12pt;
}

.select-container {
  margin: 0 8px 0 8px; /* Add spacing between "show" and the dropdown */
}
.selecter {
  border: 1px solid lightgray;
  font-size: inherit;
  border-radius: 5px;
  padding: 3px;
  background-color: white;
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
</style>
