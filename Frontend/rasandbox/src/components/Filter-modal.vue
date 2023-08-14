<template>
  <modal-bert @close="$emit('close')">
    <template #main-content>
      <div class="filter-title">Filters</div>
      <div class="columns">
        <div class="column is-one-fourth">
          <div class="select">
            <select
              name="Company"
              :value="filter.company"
              @update="handleGenreChange(filter.company)"
            >
              <option value="" disabled selected>Select a company...</option>
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
            <select
              name="Genre"
              :value="filter.genre"
              @change="handleGenreChange(filter.genre)"
            >
              <option value="" disabled selected>Select a genre...</option>
              <option v-for="genre in genres" :key="genre" :value="genre">
                {{ genre }}
              </option>
            </select>
          </div>
        </div>
        <div class="column is-one-fourth">
          <vue-slider
            class="vue-slider"
            v-model="sliderValues"
            :min="0"
            :max="100"
            :interval="1"
            @dragging="handleSliderDragging"
            @change="handlePriceRangeInput"
          >
            <template #tooltip="{ value }">
              <div class="custom-tooltip">{{ value }}€</div>
            </template>
          </vue-slider>
        </div>
        <div class="column is-one-fourth">
          <span>Release date:</span>
          <input
            type="date"
            class="releaseDatePicker"
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
    handleCompanyChange(val: any) {
      console.log(val);
      if (val === "") {
        this.filter.company = "";
      } else {
        this.filter.company = val;
      }
    },
    handleGenreChange(val: any) {
      if (val === "") {
        this.filter.genre = "";
      } else {
        this.filter.genre = val;
      }
    },
  },
  computed: {
    themeClass() {
      return this.$store.getters.getTheme;
    },
  },
});
</script>
<style lang="scss" scoped>
@import "../../shared/themes.scss";
.column.is-one-fourth {
  margin: 0 1rem 0 1rem;

  &:first-child {
    margin: 0 1rem 0 0;
  }
  &:last-child {
    margin: 0 0 0 1rem;
  }
}
.slider-labels {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 0.5rem;
}
.select,
.vue-slider {
  width: 100%;
  margin-bottom: 1rem;
}
@media (max-width: 768px) {
  /* Adjust column widths for smaller screens */
  .column.is-one-fourth {
    flex: none;
    width: 100%;
  }
}
.button.is-success {
  margin-right: 8px;
}
.filter-title {
  font-size: 20pt;
  font-weight: 500;
  margin-bottom: 15px;
}
.releaseDatePicker {
  border: 1px solid grey;
  padding: 8px;
  border-radius: 5px;
}
</style>
