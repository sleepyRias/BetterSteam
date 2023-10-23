import { Game } from "../../src/components";

export interface User {
  id: number;
  name: string;
  username: string;
  favoriteGamesList: Game[];
  colorScheme: string;
  country: string;
  emailAdress: string;
  phoneNumber: string;
  priceThreshold: number;
}
