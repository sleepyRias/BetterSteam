<template>
  <div>Super Secret Page</div>
</template>
<script lang="ts">
import Vue from "vue";
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
import axios from "axios";
import Cookies from "js-cookie";
const repo = new SteamRepositoryAxios(axios);
export default Vue.extend({
  name: "SuperSecret",
  data() {
    return {};
  },
  methods: {
    async verify() {
      const token = Cookies.get("token");
      if (token === undefined) {
        return false;
      }
      const response = await repo.verify(token);
      if (response.isValid) {
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
