<template>
  <div class="gameBox">
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
    favGameClass(): string {
      return this.isFavorited ? "fa-solid" : "fa-regular";
    },
  },
});
</script>
<style lang="scss" scoped>
.gameBox {
  // ONG das muss anders werden my eyes are bleeding
  border: 1px solid black;
  margin: 10px 5px 0px 5px;
  position: relative;
}
</style>
