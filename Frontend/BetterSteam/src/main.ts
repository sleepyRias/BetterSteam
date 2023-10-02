import App from "./App.vue";
import "./style/main.scss";
import {
  Vue,
  Vuex,
  storeOptions,
  VueRouter,
  routes,
  CookieValidation,
} from "./components/";

Vue.use(Vuex);
Vue.use(VueRouter);

Vue.config.productionTip = false;

const router = new VueRouter({
  mode: "history",
  routes,
});

router.beforeEach(async (to, from, next) => {
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if ((await CookieValidation()) === false) {
      next("/login");
      // Redirect to login page if not authenticated
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
