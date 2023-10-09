<template>
  <div class="modal is-active">
    <div class="modal-background"></div>
    <div class="modal-content">
      <div class="box" :class="themeClass">
        <slot name="main-content" />
      </div>
    </div>
    <button class="close-modal-button" @click="$emit('close')">
      <i class="fas fa-times fa-2x" />
    </button>
  </div>
</template>
<script lang="ts">
import { Vue } from "./";
export default Vue.extend({
  name: "ModalBert",
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
@import "../style/main.scss";
.close-modal-button {
  position: absolute;
  top: 0;
  right: 0;
  color: white;
  background: none;
  border: none;
  margin: 5px 5px;
  cursor: pointer;
}

.close-modal-button:hover {
  color: darkred;
}
</style>
