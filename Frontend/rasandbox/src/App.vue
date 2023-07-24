<template>
  <div class="betterSteam" :class="themeClass">
    <user-modal v-if="showUser" @close="showUser = false" />
    <filter-modal
      v-if="showFilter"
      @close="showFilter = false"
      @submit="updateFilter"
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
    <h3>show {{ amountPerPage }} items per page</h3>
    <div class="buttons has-addons">
      <button class="button" @click="amountPerPage = 20">20 items</button>
      <button class="button" @click="amountPerPage = 40">40 items</button>
      <button class="button" @click="amountPerPage = 60">60 items</button>
    </div>
    <div class="field is-grouped">
      <button class="button" @click="prevPage" :disabled="filter.page == 1">
        <span>
          <i class="fa-solid fa-arrow-left"></i>
        </span>
      </button>
      <button class="button is-static">
        {{ page }}
      </button>
      <button class="button">
        <span>
          <i class="fa-solid fa-arrow-right" @click="nextPage"></i>
        </span>
      </button>
    </div>
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
        <div class="gameBox">
          <ul>
            <!-- digga was ist das mach das mal anders digga -->
            <li>{{ game.name }}</li>
            <li>{{ game.price }}€</li>
            <li>{{ game.company }}</li>
            <li>{{ game.releaseDate }}</li>
          </ul>
          <button
            class="betterSteamButton--favorite"
            @click="isFavorited = !isFavorited"
          >
            <span class="icon">
              <i :class="favGameClass" class="fa-star fa-lg" />
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
import { RawLocation } from "vue-router";
export default Vue.extend({
  name: "App",
  components: {
    UserModal,
    FilterModal,
  },
  data() {
    return {
      amountPerPage: 20,
      gamesList: [] as Game[],
      showUser: false,
      showFilter: false,
      isFavorited: false,
      isGamesLoading: false,
    };
  },
  methods: {
    updateFilter(filter: GameFilter) {
      this.filter = { ...this.filter, ...filter };
    },
    isInPricerange(price: number): boolean {
      if (!(this.filter.maxPrice && this.filter.minPrice)) {
        return true;
      }
      return price >= this.filter.minPrice && price <= this.filter.maxPrice;
    },
    isInGenre(genres: string[]): boolean {
      if (!this.filter.genre) {
        return true;
      }
      return genres.includes(this.filter.genre);
    },
    async getGames() {
      this.gamesList = await repo.getGames(this.filter);
    },
    nextPage() {
      this.routeTo({
        path: "/items",
        query: { page: String(this.filter.page ?? 1 + 1) },
      });
    },
    prevPage() {
      this.routeTo({
        path: "/items",
        query: { page: String(this.filter.page ?? 1 - 1) },
      });
    },
    updateRoute() {
      this.routeTo(this.routeToItems());
    },
    routeToItems(): RawLocation {
      return {
        path: "/items",
        query: {
          page: String(this.$route.query.page),
          name: this.$route.query.name,
          company: this.$route.query.company,
          genre: this.$route.query.genre,
          minPrice: String(this.$route.query.minPrice),
          maxPrice: String(this.$route.query.maxPrice),
          releaseDate: this.$route.query.releaseDate,
        },
      };
    },
    async routeTo(location: RawLocation) {
      try {
        await this.$router.push(location);
      } catch (err) {
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-ignore
        if (err?.name !== "NavigationDuplicated") {
          throw err;
        }
      }
    },
  },
  computed: {
    themeClass(): string {
      return this.$store.getters.getTheme;
    },
    favGameClass(): string {
      return this.isFavorited ? "fa-solid" : "fa-regular";
    },
    filter(): GameFilter {
      return {
        page: this.page,
        company: this.company,
        name: this.name,
        genre: this.genre,
        maxPrice: this.maxPrice,
        minPrice: this.minPrice,
        releaseDate: this.releaseDate,
      };
    },
  },
  watch: {
    filter: {
      deep: true,
      handler() {
        this.getGames();
        this.updateRoute();
      },
    },
  },
  props: {
    page: { type: Number, default: undefined },
    name: { type: String, default: undefined },
    company: { type: String, default: undefined },
    genre: { type: String, default: undefined },
    maxPrice: { type: Number, default: undefined },
    minPrice: { type: Number, default: undefined },
    releaseDate: { type: String, default: undefined },
  },
  mounted() {
    this.getGames();
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
  // ONG das muss anders werden my eyes are bleeding
  border: 1px solid black;
  margin: 10px 5px 0px 5px;
  position: relative;
}
</style>
