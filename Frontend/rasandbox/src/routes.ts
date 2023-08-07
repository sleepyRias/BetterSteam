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
      page: route.query.page || "",
      genre: route.query.genre || "",
    }),
  },
];

export default routes;
