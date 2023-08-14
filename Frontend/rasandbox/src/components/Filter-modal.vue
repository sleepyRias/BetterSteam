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
import Vue from "vue";
import { GameFilter } from "../../shared/interfaces/filters";
import modalBert from "./modalBert.vue";

export default Vue.extend({
  name: "FilterModal",
  components: {
    modalBert,
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

      const queryParams = {
        page: this.filter.page.toString(),
        company: this.filter.company,
        genre: this.filter.genre,
        name: this.filter.name,
        releaseDate: this.filter.releaseDate,
        minPrice: this.filter.minPrice.toString(),
        maxPrice: this.filter.maxPrice.toString(),
      };

      this.$router.push({ path: "/games", query: queryParams });
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
        pageSize: 20,
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
.modal-content {
  width: auto;
}
</style>
