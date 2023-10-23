import { VueI18n } from "../../src/components";

export const i18n = new VueI18n({
  locale: "en",
  messages: {
    en: require("./languages/en.json"),
    es: require("./languages/es.json"),
  },
});
