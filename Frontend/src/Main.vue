<template>
  <div class="better-steam" :class="themeClass">
    <filter-modal
      v-if="showFilter"
      @close="showFilter = false"
      @submit="updateFilter"
      @clearFilter="clearFilter"
    />
    <div class="main-header">
      <h1 class="main-title">Sandbox Project</h1>
      <button @click="goToProfile" class="better-steam-button--user">
        <i class="fa-regular fa-user fa-2x" />
      </button>
    </div>
    <button @click="toggleDarkmode" class="better-steam-button--theme">
      <i :class="darkOrLightClass" class="fa-solid fa-2xl" />
    </button>
    <div class="search-with-filters is-rounded">
      <div class="control has-icons-right">
        <input
          class="input"
          type="text"
          placeholder="Search any Game"
          v-model="filter.name"
        />
        <!-- arme API aber egal ist niklas problem net meins -->
        <span class="icon is-small is-right">
          <i class="fa-solid fa-magnifying-glass" />
        </span>
      </div>
      <button class="button" @click="showFilter = true">
        <span class="icon">
          <i class="fa-solid fa-filter" />
        </span>
        <span>Filter</span>
      </button>
    </div>
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
        <game-box
          :Game="game"
          @addFavourite="addFavourite"
          @removeFavourite="removeFavourite"
          @addWishlist="addWishlist"
          @removeWishlist="removeWishlist"
        />
      </div>
    </div>
    <button v-if="showUpButton" @click="scrollToTop" class="up-button">
      <i class="fa-solid fa-arrow-up"></i>
    </button>
  </div>
</template>

<script lang="ts">
import { SteamRepositoryAxios } from "../public/axios/SteamRepositoryAxios";
const repo = new SteamRepositoryAxios(axios);
import {
  Vue,
  axios,
  Game,
  FilterModal,
  GameBox,
  Cookies,
  GameFilter,
  Themes,
} from "./components/";
import { mapState } from "vuex";
export default Vue.extend({
  // eslint-disable-next-line vue/multi-word-component-names
  name: "Main",
  components: {
    FilterModal,
    GameBox,
  },
  data() {
    return {
      showFilter: false,
      totalGamesCount: 0,
      showUpButton: false,
      gamesList: [] as Game[],
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
    checkScroll() {
      this.showUpButton = window.scrollY > 300; // Adjust the threshold as needed
    },

    scrollToTop() {
      window.scrollTo({ top: 0, behavior: "smooth" });
    },
    async goToProfile() {
      const token = Cookies.get("token");
      if (token === undefined) {
        this.$router.push("/login");
      }
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      const response = await repo.verify(token!);
      if (response.isValid) {
        this.$router.push("/user");
      } else {
        this.$router.push("/login");
      }
    },
    toggleDarkmode() {
      switch (this.$store.getters.getTheme) {
        case Themes.light:
          this.$store.dispatch("setTheme", Themes.dark);
          break;
        case Themes.dark:
          this.$store.dispatch("setTheme", Themes.light);
          break;
        case Themes.red:
          break;
        default:
          this.$store.dispatch("setTheme", Themes.light);
      }
    },
    addFavourite(id: number, token: string) {
      repo.addFavoriteGame(token, id.toString());
    },
    removeFavourite(id: number, token: string) {
      repo.removeFavouriteGame(token, id.toString());
    },
    addWishlist(id: number, token: string) {
      repo.addToWishlist(token, id.toString());
    },
    removeWishlist(id: number, token: string) {
      repo.removeFromWishlist(token, id.toString());
    },
    async getWishlist() {
      const token = Cookies.get("token");
      if (token !== undefined) {
        const response = await repo.getWishlist(token);
        // eslint-disable-next-line no-console
        console.log(response);
      }
    },
  },
  computed: {
    ...mapState(["theme", "wishlist"]),
    themeClass(): string {
      return this.$store.getters.getTheme;
    },
    darkOrLightClass(): string {
      return this.$store.getters.getTheme === "dark-theme"
        ? "fa-sun"
        : "fa-moon";
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
    this.$store.dispatch("fetchWishlist");
  },
  mounted() {
    window.addEventListener("scroll", this.checkScroll);
  },

  beforeDestroy() {
    window.removeEventListener("scroll", this.checkScroll);
  },
});
</script>

<style lang="scss">
@import "../public/themes.scss";
html,
body {
  height: 100vh;
}
.better-steam {
  height: 100%;
  padding: 0 20px 0 20px;
}
.better-steam-button {
  border: none;
  background: none;
  cursor: pointer;
  &--user {
    @extend .better-steam-button;
    color: #1b1d9e;
    margin: 5px 5px 0 0;
  }
  &--favorite {
    @extend .better-steam-button;
    position: absolute !important;
    top: 0;
    right: 0;
    padding: 0;
    margin: 4px 6px;
    color: #fcd303;
  }
  &--wishlist {
    @extend .better-steam-button;
    color: #ff0000;
    padding: 0;
    margin-top: 15px;
    text-decoration: dashed;
  }
  &--theme {
    @extend .better-steam-button;
    margin: 7px 100px 0 0;
    position: absolute;
    top: 0;
    right: 0;
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
.up-button {
  position: fixed;
  bottom: 35px; /* Adjust the distance from the bottom as needed */
  right: 20px; /* Adjust the distance from the right as needed */
  z-index: 9999; /* Make sure it's above other elements */
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  background-color: #fff;
  color: #333;
  border: 1px solid #333;
  border-radius: 50%;
  cursor: pointer;

  &:hover {
    transform: scale(1.1);
    background-color: #333;
    color: #fff;
  }
}
.search-with-filters {
  display: flex;
  justify-content: center;
}
.fa-sun {
  color: white;
}
</style>
