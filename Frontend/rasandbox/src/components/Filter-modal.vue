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
          <vue-slider
            v-model="sliderValues"
            :min="0"
            :max="100"
            :interval="1"
            @dragging="handleSliderDragging"
            @change="handlePriceRangeInput"
          />
          <div class="slider-lables">
            <span>{{ sliderValues[0] }}€</span>
            <span> {{ sliderValues[1] }}€ </span>
          </div>
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
import VueSlider from "vue-slider-component";
import "vue-slider-component/theme/default.css";

export default Vue.extend({
  name: "FilterModal",
  components: {
    modalBert,
    VueSlider,
  },
  data() {
    return {
      showDropDown: false,
      sliderValues: [0, 100],
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
      this.filter.minPrice = this.sliderValues[0];
      this.filter.maxPrice = this.sliderValues[1];
      this.$emit("submit", this.filter);

      const queryParams = {
        page: this.filter.page?.toString(),
        company: this.filter.company,
        genre: this.filter.genre,
        name: this.filter.name,
        releaseDate: this.filter.releaseDate,
        minPrice: this.filter.minPrice?.toString(),
        maxPrice: this.filter.maxPrice?.toString(),
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
    handleSliderDragging() {
      if (this.sliderValues[0] > this.sliderValues[1]) {
        // If the first slider is dragged past the second slider, adjust the values
        this.sliderValues[0] = this.sliderValues[1];
      } else if (this.sliderValues[1] < this.sliderValues[0]) {
        // If the second slider is dragged past the first slider, adjust the values
        this.sliderValues[1] = this.sliderValues[0];
      }
    },
    handlePriceRangeInput() {
      this.filter.minPrice = this.sliderValues[0];
      this.filter.maxPrice = this.sliderValues[1];
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
.slider-labels {
  display: flex;
  justify-content: space-between;
  margin-top: 10px;
}
</style>
