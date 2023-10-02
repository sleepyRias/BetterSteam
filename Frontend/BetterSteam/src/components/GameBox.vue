<template>
  <div class="gameBox" :class="(themeClass, { 'hover-effect': enableHover })">
    <div class="columns">
      <div class="column is-one-third">
        <figure class="image is-128x128">
          <img :src="imageUrl" />
        </figure>
      </div>
      <div class="column is-two-third gameinfo">
        <p class="gametitle">{{ Game.name }}</p>
        <p class="gamecompany">{{ Game.company }}</p>
        <span class="gameprice">{{ `${Game.price.toFixed(2)}€` }} </span>
        <p class="gamedate">{{ formattedDate }}</p>
      </div>
    </div>
    <button
      class="betterSteamButton--favorite"
      @click="isFavorited = !isFavorited"
    >
      <span class="icon" v-if="Favoriteable">
        <i :class="favGameClass" class="fa-star fa-lg" />
      </span>
    </button>
  </div>
</template>
<script lang="ts">
import { Vue } from "./";
export default Vue.extend({
  name: "GameBox",
  props: {
    Game: { type: Object, default: undefined },
    enableHover: { type: Boolean, default: true },
    Favoriteable: { type: Boolean, default: true },
  },
  data() {
    return {
      isFavorited: false,
      formattedDate: "",
      imageUrl: "",
    };
  },
  computed: {
    themeClass(): string {
      return this.$store.getters.getTheme;
    },
    favGameClass(): string {
      return this.isFavorited ? "fa-solid" : "fa-regular";
    },
  },
  methods: {
    formatDate(dateString: string) {
      const parts = dateString.split("-");
      if (parts.length !== 3) return dateString;
      return `${parts[2]}.${parts[1]}.${parts[0]}`;
    },
    async getRandomPreview() {
      try {
        const response = await fetch(
          "https://localhost:7091/api/image/GetRandomPreview"
        );
        if (!response.ok) {
          throw new Error(
            "Netzwerkantwort war nicht ok " + response.statusText
          );
        }
        const blob = await response.blob();
        this.imageUrl = URL.createObjectURL(blob);
      } catch (error) {
        return;
      }
    },
  },
  mounted() {
    this.formattedDate = this.formatDate(this.Game.releaseDate);
    this.getRandomPreview();
  },
});
</script>
<style lang="scss" scoped>
@import "../../shared/themes.scss";
.gameBox {
  box-shadow: 1px 1px 1px 1px rgba(0, 0, 0, 0.2);
  margin: 10px 5px 0px 5px;
  position: relative;
  border-radius: 5px;
  padding: 6px;
  transition: transform 0.2s ease;

  &.hover-effect:hover {
    box-shadow: 0 5px 15px 0 rgba(0, 0, 0, 0.8);
    transform: scale(1.08);
    z-index: 500;
  }
}
.gameprice {
  position: absolute;
  bottom: 0;
  right: 0;
  color: darkgreen;
  font-size: 18pt;
  font-weight: 600;
}
.gametitle {
  font-size: 15pt;
  font-family: Avenir, Helvetica, Arial, sans-serif;
}
.gamecompany {
  font-size: small;
}
.gameinfo {
  position: relative;
}
.gamedate {
  position: absolute;
  bottom: 0;
}
.light-theme {
  background-color: $secondary-light-theme-color;
}
.dark-theme {
  background-color: $secondary-dark-theme-color;
}
</style>
