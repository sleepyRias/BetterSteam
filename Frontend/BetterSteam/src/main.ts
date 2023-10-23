import App from "./App.vue";
import "./style/main.scss";
import {
  Vue,
  Vuex,
  storeOptions,
  VueRouter,
  routes,
  CookieValidation,
  VueI18n,
} from "./components/";

Vue.use(Vuex);
Vue.use(VueRouter);
Vue.use(VueI18n);

Vue.config.productionTip = false;

const router = new VueRouter({
  mode: "history",
  routes,
});

const i18n = new VueI18n({
  locale: "en",
  messages: {
    en: require("./languages/en.json"),
    es: require("./languages/es.json"),
  },
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
  i18n,
  render: (h) => h(App),
  store: new Vuex.Store(storeOptions),
}).$mount("#app");
