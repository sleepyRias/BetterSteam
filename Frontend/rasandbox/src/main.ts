import Vue from "vue";
import Vuex from "vuex";
import App from "./App.vue";
import VueRouter from "vue-router";
import { storeOptions } from "./store/store";
import "@/assets/main.scss";

Vue.use(Vuex);
Vue.use(VueRouter);
Vue.config.productionTip = false;

const routes = [
  { path: "/items", component: App },
  // more routes hier
];

const router = new VueRouter({
  routes,
});

new Vue({
  router,
  render: (h) => h(App),
  store: new Vuex.Store(storeOptions),
}).$mount("#app");
