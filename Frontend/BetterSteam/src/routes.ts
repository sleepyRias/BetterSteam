import LoginWindowVue from "./components/LoginWindow.vue";
import Main from "./Main.vue";
import UserWindow from "./components/UserWindow.vue";
import CreateNewUser from "./components/CreateNewUser.vue";
import SuperSecret from "./components/SuperSecret.vue";

const routes = [
  {
    path: "/",
    redirect: "/games",
  },
  {
    path: "/games",
    name: "GameBox",
    component: Main,
    props: (route: any) => ({
      page: route.query.page || 1,
      company: route.query.company || "",
      genre: route.query.genre || "",
      name: route.query.name || "",
      releaseDate: route.query.releaseDate || "",
      minPrice: route.query.minPrice || 0,
      maxPrice: route.query.maxPrice || 100,
    }),
  },
  {
    path: "/login",
    component: LoginWindowVue,
  },
  {
    path: "/user",
    component: UserWindow,
    meta: { requiresAuth: true }, // This route requires authentication
  },
  {
    path: "/create",
    component: CreateNewUser,
  },
  {
    path: "/robert",
    component: SuperSecret,
  },
];

export default routes;
