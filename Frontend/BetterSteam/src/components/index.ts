// Exporting third-party libraries
export { default as Vue } from "vue";
export { default as Vuex } from "vuex";
export { default as axios } from "axios";
export { default as Cookies } from "js-cookie";
export { default as VueRouter } from "vue-router";
export { default as VueI18n } from "vue-i18n";

// Exporting interfaces
export { Game } from "../../shared/interfaces/Game";
export { GameFilter } from "../../shared/interfaces/Filters";
export { Themes } from "../../shared/interfaces/Themes";
export { User } from "../../shared/interfaces/User";
export * from "../../shared/interfaces/BetterSteamResponse";
export { default as CookieValidation } from "../../shared/CookieValidation";
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
export { SteamRepository } from "../../shared/repos/SteamRepository";

// Exporting languages
export { i18n } from "../../shared/internationalization/i18n";
