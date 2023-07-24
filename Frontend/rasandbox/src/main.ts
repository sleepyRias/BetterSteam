import Vue from "vue";
import Vuex from "vuex";
import App from "./App.vue";
import VueRouter, { Route } from "vue-router";
import { storeOptions } from "./store/store";
import "@/assets/main.scss";

Vue.use(Vuex);
Vue.use(VueRouter);
Vue.config.productionTip = false;

const routes = [
  {
    path: "/",
    component: App,
    props: (route: any) => ({
      page: parseInt(route.query.page as string),
      name: route.query.name,
      company: route.query.company,
      genre: route.query.genre,
      maxPrice: parseInt(route.query.maxPrice as string),
      minPrice: parseInt(route.query.minPrice as string),
      releaseDate: route.query.releaseDate,
    }),
  },
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
