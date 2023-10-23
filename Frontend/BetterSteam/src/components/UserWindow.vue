<template>
  <div class="columns">
    <div class="column is-auto" />
    <div class="column is-two-thirds">
      <div class="user-controls">
        <figure class="image is-128x128">
          <img src="https://bulma.io/images/placeholders/128x128.png" />
        </figure>
        <div>Username: {{ username }}</div>
        <div class="control has-icons-left">
          <div class="select">
            <select v-model="country">
              <option selected value="">Country</option>
              <option value="USA">USA</option>
              <option value="United Kingdom">United Kingdom</option>
              <option value="Germany">Germany</option>
              <option value="France">France</option>
              <option value="Italy">Italy</option>
              <option value="Poland">Poland</option>
              <option value="Spain">Spain</option>
              <option value="Russia">Russia</option>
              <option value="China">China</option>
            </select>
          </div>
          <div class="icon is-small is-left">
            <i class="fas fa-globe"></i>
          </div>
        </div>
        <div class="field">
          <p class="control has-icons-left has-icons-right">
            <input
              class="input"
              type="email"
              placeholder="Email"
              v-model="email"
              @input="checkIfEmailIsValid"
              :class="{ 'is-success': checkIfEmailIsValid() }"
            />
            <span class="icon is-small is-left">
              <i class="fas fa-envelope"></i>
            </span>
            <span class="icon is-small is-right">
              <i class="fas fa-check"></i>
            </span>
          </p>
        </div>
        <div class="field">
          <p class="control has-icons-left has-icons-right">
            <input
              class="input"
              type="text"
              placeholder="Phonenumber"
              v-model="phonenumber"
              @input="checkIfPhoneIsValid"
              :class="{ 'is-success': checkIfPhoneIsValid() }"
            />
            <span class="icon is-small is-left">
              <i class="fas fa-phone"></i>
            </span>
            <span class="icon is-small is-right">
              <i class="fas fa-check"></i>
            </span>
          </p>
        </div>
        <div class="price-threshold-slider">
          <span>
            How much can a game be above and below your selected price range
          </span>
          <input
            type="range"
            min="0"
            max="100"
            value="20"
            v-model="priceThreshold"
          />
          <span>{{ priceThreshold }}%</span>
        </div>
      </div>
    </div>
    <div class="column is-auto"></div>
  </div>
</template>
<script lang="ts">
import { Vue, axios, Cookies } from "./";
import { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
const repo = new SteamRepositoryAxios(axios);
export default Vue.extend({
  name: "SuperSecret",
  data() {
    return {
      username: "",
      country: "",
      email: "",
      phonenumber: "",
      priceThreshold: 0,
    };
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
    async getUsername() {
      const token = Cookies.get("token");
      if (token === undefined) return;
      this.username = (await repo.getNameFromToken(token)).name;
    },
    checkIfEmailIsValid() {
      return this.email.includes("@") && this.email.endsWith(".com");
    },
    checkIfPhoneIsValid() {
      return this.phonenumber.startsWith("+") && this.phonenumber.length > 8;
    },
  },
  beforeMount() {
    if (!this.verify()) {
      this.$router.push("/login");
    }
  },
  mounted() {
    this.getUsername();
  },
});
</script>
<style lang="scss" scoped>
.user-controls {
  margin: 10px 0 0 10px;
}
</style>
