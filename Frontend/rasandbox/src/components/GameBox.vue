<template>
  <div class="gameBox" :class="themeClass">
    <ul>
      <!-- digga was ist das mach das mal anders digga -->
      <li>{{ Game.name }}</li>
      <li>{{ Game.price }}€</li>
      <li>{{ Game.company }}</li>
      <li>{{ Game.releaseDate }}</li>
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
</template>
<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  name: "GameBox",
  props: {
    Game: { type: Object, default: undefined },
  },
  data() {
    return {
      isFavorited: false,
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
});
</script>
<style lang="scss" scoped>
@import "../../shared/themes.scss";
.gameBox {
  // ONG das muss anders werden my eyes are bleeding
  box-shadow: 1px 1px 1px 1px rgba(0, 0, 0, 0.2);
  margin: 10px 5px 0px 5px;
  position: relative;
  border-radius: 5px;
  padding: 0px 5px 0px 10px;
  transition: transform 0.2s ease;

  &:hover {
    box-shadow: 0 5px 15px 0 rgba(0, 0, 0, 0.8);
    transform: scale(1.08);
    z-index: 1000;
  }
}
.light-theme {
  background-color: $secondary-light-theme-color;
}
.dark-theme {
  background-color: $secondary-dark-theme-color;
}
</style>
