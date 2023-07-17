<template>
  <div class="modal">
    <div class="modal-background"></div>
    <div class="modal-content">
      <div class="box" :class="themeClass">
        <slot name="main-content" />
      </div>
    </div>
    <button class="modal-close is-large" @click="$emit('close')">close</button>
  </div>
</template>
<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  name: "modalBert",
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
