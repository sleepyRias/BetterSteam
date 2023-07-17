import Vue from "vue";
import Vuex from "vuex";
import App from "./App.vue";
import { storeOptions } from "./store/store";

Vue.use(Vuex);
// store hier rein
Vue.config.productionTip = false;
require("@/assets/main.scss");
// robert mag das nicht main erstmal verscheieben und dann "importieren"

new Vue({
  render: (h) => h(App),
  store: new Vuex.Store(storeOptions),
}).$mount("#app");
