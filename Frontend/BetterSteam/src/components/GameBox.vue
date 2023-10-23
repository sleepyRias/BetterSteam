<template>
  <div :class="[themeClass, { 'hover-effect': enableHover }, 'game-box']">
    <div class="columns is-mobile">
      <div class="column is-one-third">
        <figure class="image is-256x256">
          <img :src="imageUrl" />
        </figure>
      </div>
      <div class="column is-two-third game-info">
        <p class="game-title">{{ Game.name }}</p>
        <p class="game-company">{{ Game.company }}</p>
        <button
          class="better-steam-button--wishlist"
          @click="handleWishlist"
          v-if="isAllowedToFavAndWish"
        >
          <span class="icon" v-if="wishlistable">
            <i :class="[wishGameClass, 'fa-bookmark', 'fa-xl']" />
          </span>
        </button>
        <span class="game-price">{{ `${Game.price.toFixed(2)}€` }} </span>
        <p class="game-date">{{ formattedDate }}</p>
      </div>
    </div>
    <button
      v-if="isAllowedToFavAndWish"
      class="better-steam-button--favorite"
      @click="handleFavorite"
    >
      <span class="icon" v-if="favoriteable">
        <i :class="[favGameClass, 'fa-star', 'fa-xl']" />
      </span>
    </button>
  </div>
</template>
<script lang="ts">
import { Vue, WishlistResponse } from "./";
export default Vue.extend({
  name: "GameBox",
  props: {
    Game: { type: Object, default: undefined },
    enableHover: { type: Boolean, default: true },
    favoriteable: { type: Boolean, default: true },
    wishlistable: { type: Boolean, default: true },
  },
  data() {
    return {
      isFavorited: false,
      formattedDate: "",
      isWishlisted: false,
      isAllowedToFavAndWish: false,
      imageUrl: "",
      wishlist: [] as WishlistResponse[],
    };
  },
  methods: {
    async handleFavorite() {
      const token = this.$store.getters.getToken;
      try {
        await this.$store.dispatch("verifyToken", token);
        if (this.isFavorited) {
          this.$emit("removeFavourite", this.Game.id, token);
        } else {
          this.$emit("addFavourite", this.Game.id, token);
        }
      } catch (error) {
        // eigentlich müsste wenn der token nicht valid ist was anders passieren
        alert(error);
      }
      this.isFavorited = !this.isFavorited;
    },
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
    async handleWishlist() {
      const token = this.$store.getters.getToken;
      try {
        await this.$store.dispatch("verifyToken", token);
        if (this.isWishlisted) {
          this.isWishlisted = !this.isWishlisted;
          this.$emit("removeWishlist", this.Game.id, token);
        } else {
          this.isWishlisted = !this.isWishlisted;
          this.$emit("addWishlist", this.Game.id, token);
        }
      } catch (error) {
        // eigentlich müsste wenn der token nicht valid ist was anders passieren
        alert(error);
      }
    },
    fetchWishlist() {
      this.wishlist = this.$store.getters.getWishlist;
      for (var gameID of this.wishlist) {
        if (gameID.gameId === this.Game.id) {
          this.isWishlisted = true;
        }
      }
    },
    async toggleFavAndWishButton() {
      await this.$store
        .dispatch("verifyToken", this.$store.getters.getToken)
        .then((data: boolean) => {
          if (data === true) {
            this.isAllowedToFavAndWish = true;
          }
        });
    },
  },
  computed: {
    themeClass(): string {
      return this.$store.getters.getTheme;
    },
    favGameClass(): string {
      return this.isFavorited ? "fa-solid" : "fa-regular";
    },
    wishGameClass(): string {
      return this.isWishlisted ? "fa-solid" : "fa-regular";
    },
  },
  mounted() {
    this.toggleFavAndWishButton();
    this.formattedDate = this.formatDate(this.Game.releaseDate);
    this.getRandomPreview();
  },
  created() {
    this.fetchWishlist();
  },
});
</script>
<style lang="scss" scoped>
@import "../../shared/themes.scss";
@import "../style/main.scss";
.game-box {
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
.game-price {
  position: absolute;
  bottom: 0;
  right: 0;
  color: darkgreen;
  font-size: 18pt;
  font-weight: 600;
}
.game-title {
  font-size: 15pt;
  font-family: Avenir, Helvetica, Arial, sans-serif;
}
.game-company {
  font-size: small;
}
.game-info {
  position: relative;
  margin-left: 6px;
}
.game-date {
  position: absolute;
  bottom: 0;
}
.columns {
  margin: 0px !important;
}
</style>
