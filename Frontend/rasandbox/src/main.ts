import Vue from "vue";
import Vuex from "vuex";
import App from "./App.vue";
import { storeOptions } from "./store/store";
import VueRouter from "vue-router";
import "./assets/main.scss";
import routes from "./routes";

Vue.use(Vuex);
Vue.use(VueRouter);

Vue.config.productionTip = false;

const router = new VueRouter({
  mode: "history",
  routes,
});

router.beforeEach((to, from, next) => {
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (!storeOptions.getters.isAuthenticated) {
      // Redirect to login page if not authenticated
      next("/login");
    } else {
      next();
    }
  } else {
    next();
  }
});

new Vue({
  router,
  render: (h) => h(App),
  store: new Vuex.Store(storeOptions),
}).$mount("#app");
