export interface GameFilter {
  page: number;
  name: string;
  company: string;
  genre: string;
  minPrice: number;
  maxPrice: number;
  releaseDate: string;
  pageSize: number;
}
