<template>
  <modal-bert @close="$emit('close')">
    <template #main-content>
      <h1>Filters</h1>
      <div class="columns">
        <div class="column is-one-fourth">
          <div class="select">
            <select name="Company" v-model="filter.company">
              <option
                v-for="company in companies"
                :key="company"
                :value="company"
              >
                {{ company }}
              </option>
            </select>
          </div>
        </div>
        <div class="column is-one-fourth">
          <div class="select">
            <select name="Genre" v-model="filter.genre">
              <option v-for="genre in genres" :key="genre" :value="genre">
                {{ genre }}
              </option>
            </select>
          </div>
        </div>
        <div class="column is-one-fourth">
          <span>Minimum Preis</span>
          <input
            type="range"
            name="minPrice"
            min="0"
            max="20"
            step="1"
            v-model="filter.minPrice"
          />
          {{ filter.minPrice }} €
          <span>Maximal Preis</span>
          <input
            type="range"
            name="minPrice"
            min="30"
            max="100"
            step="1"
            v-model="filter.maxPrice"
          />
          {{ filter.maxPrice }} €
        </div>
        <div class="column is-one-fourth">
          <label for="start">Release date:</label>
          <input
            type="date"
            id="start"
            name="trip-start"
            value="2023-06-12"
            min="1999-01-01"
            max="2100-12-31"
            v-model="filter.releaseDate"
          />
        </div>
      </div>
      <button class="button is-success" @click="submitFilter">Submit</button>
      <button class="button is-danger" @click="clearFilter">
        Clear Filter
      </button>
    </template>
  </modal-bert>
</template>

<script lang="ts">
import { Vue, GameFilter, ModalBert } from "./";
export default Vue.extend({
  name: "FilterModal",
  components: {
    ModalBert,
  },
  data() {
    return {
      showDropDown: false,
      company: "",
      genre: "",
      minPrice: 0,
      maxPrice: 0,
      releaseDate: "",
      filter: {} as GameFilter,
      genres: [
        "Horror",
        "Abenteuer",
        "Physik",
        "Survival",
        "Simulation",
        "Aufbauspiel",
        "Robert hat Spaß",
        "RPG",
        "MMO",
        "Krieg",
        "Weltraum",
        "Story",
        "Cyberpunk",
        "FPS",
      ],
      companies: [
        "Valve",
        "Adobe",
        "343 Studios",
        "Paradox Interactive",
        "CD Projekt Red",
        "RA Micro Software AG",
        "Respawn",
      ],
    };
  },
  methods: {
    submitFilter() {
      this.$emit("submit", this.filter);

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
      this.$router.push({ path: "/games", query: queryParameters });

      this.$emit("close");
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
        pageSize: 30,
      };
      this.filter = { ...defaultFilter };
      this.$router.push({ path: "/games", query: {} }); // Clear URL parameters
      this.$emit("clearFilter");
      this.$emit("close");
    },
  },
  computed: {
    themeClass() {
      return this.$store.getters.getTheme;
    },
  },
});
</script>
<style scoped>
@import "../style/main.scss";
</style>
