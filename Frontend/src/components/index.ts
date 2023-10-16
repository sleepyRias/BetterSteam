// Exporting third-party libraries
export { default as Vue } from "vue";
export { default as Vuex } from "vuex";
export { default as axios } from "axios";
export { default as Cookies } from "js-cookie";
export { default as VueRouter } from "vue-router";

// Exporting interfaces
export { Game } from "../../public/interfaces/Game";
export { GameFilter } from "../../public/interfaces/Filters";
export { Themes } from "../../public/interfaces/Themes";
export { User } from "../../public/interfaces/User";
export * from "../../public/interfaces/BetterSteamResponse";
export { default as CookieValidation } from "../../public/CookieValidation";
export { default as routes } from "../routes";

// Exporting components
export const FilterModal = () => import("./FilterModal.vue");
export const GameBox = () => import("./GameBox.vue");
export const ModalBert = () => import("./ModalBert.vue");
export const LoginWindow = () => import("./LoginWindow.vue");
export const UserWindow = () => import("./UserWindow.vue");
export const CreateNewUser = () => import("./CreateNewUser.vue");
export const SuperSecret = () => import("./SuperSecret.vue");
export const Main = () => import("../Main.vue");

// Exporting store
export { storeOptions } from "../store/store";

// Exporting repos
export { SteamRepository } from "../../public/repos/SteamRepository";
