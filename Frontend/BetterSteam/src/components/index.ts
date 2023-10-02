// Exporting third-party libraries
export { default as Vue } from "vue";
export { default as Vuex } from "vuex";
export { default as axios } from "axios";
export { default as Cookies } from "js-cookie";
export { default as moment } from "moment";
export { default as VueRouter } from "vue-router";

// Exporting interfaces
export { Game } from "../../shared/interfaces/Game";
export { GameFilter } from "../../shared/interfaces/Filters";
export * from "../../shared/interfaces/BetterSteamResponse";
export { default as CookieValidation } from "../../shared/CookieValidation";
export { default as routes } from "../routes";

// Exporting axios repositories
export { SteamRepositoryAxios } from "../../shared/axios/SteamRepositoryAxios";
export { AxiosRepository } from "../../shared/axios/AxiosRepository";

// Exporting components
export { default as FilterModal } from "./FilterModal.vue";
export { default as GameBox } from "./GameBox.vue";
export { default as ModalBert } from "./ModalBert.vue";
export { default as LoginWindow } from "./LoginWindow.vue";
export { default as UserWindow } from "./UserWindow.vue";
export { default as CreateNewUser } from "./CreateNewUser.vue";
export { default as SuperSecret } from "./SuperSecret.vue";
export { default as Main } from "../Main.vue";

// Exporting store
export { storeOptions } from "../store/store";

// Exporting repos
export { SteamRepository } from "../../shared/repos/SteamRepository";
