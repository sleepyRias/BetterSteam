<template>
  <modal-bert @close="$emit('close')">
    <template #main-content>
      <h1>Filters</h1>
      <div class="columns">
        <div class="column is-one-fourth">
          <div class="select">
            <select name="Company" v-model="filter.company">
              <option value="" selected disabled>Wähle einen Hersteller</option>
              <option value="Valve">Valve</option>
              <option value="Adobe">Adobe</option>
              <option value="343 Studios">343 Studios</option>
              <option value="Paradox Interactive">Paradox Interactive</option>
              <option value="CD Projekt Red">CD Projekt Red</option>
              <option value="RA Micro Software AG">RA Micro Software AG</option>
              <option value="Respawn">Respawn</option>
            </select>
          </div>
        </div>
        <div class="column is-one-fourth">
          <div class="select">
            <select name="Genre" v-model="filter.genre">
              <option value="" selected disabled>Wähle ein Genre</option>
              <option value="Horror">Horror</option>
              <option value="Abenteuer">Abenteuer</option>
              <option value="Physik">Physik</option>
              <option value="Survival">Survival</option>
              <option value="Simulation">Simulation</option>
              <option value="Aufbauspiel">Aufbauspiel</option>
              <option value="Robert hat Spaß">Robert hat Spaß</option>
              <option value="RPG">RPG</option>
              <option value="MMO">MMO</option>
              <option value="Krieg">Krieg</option>
              <option value="Weltraum">Weltraum</option>
              <option value="Story">Story</option>
              <option value="Cyberpunk">Cyberpunk</option>
              <option value="FPS">FPS</option>
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
      <button class="button is-danger" @click="submitFilter">
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
    };
  },
  methods: {
    submitFilter() {
      this.$emit("submit", this.filter);
      this.$emit("close");
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
  },
});
</script>
<style scoped>
.modal-content {
  width: auto;
}
</style>
