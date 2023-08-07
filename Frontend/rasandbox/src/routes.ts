import GameBox from "./components/GameBox.vue";

const routes = [
  {
    path: "/",
    redirect: "/games",
  },
  {
    path: "/games",
    name: "GameBox",
    component: GameBox,
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
];

export default routes;
