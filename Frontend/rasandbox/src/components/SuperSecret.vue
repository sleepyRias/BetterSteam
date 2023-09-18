<template>
  <div>Super Secret Page</div>
</template>
<script lang="ts">
import Vue from "vue";
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
import axios from "axios";
const repo = new SteamRepositoryAxios(axios);
export default Vue.extend({
  name: "SuperSecret",
  data() {
    return {};
  },
  methods: {
    async verify() {
      const response = await repo.verify(this.$store.state.token);
      if (response.isValid) {
        this.$store.state.isAuthenticated = true;
        return true;
      }
      return false;
    },
  },
  beforeMount() {
    if (!this.verify()) {
      this.$router.push("/login");
    }
  },
});
</script>
