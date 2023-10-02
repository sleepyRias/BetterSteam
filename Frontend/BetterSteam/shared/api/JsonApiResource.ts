export interface JsonApiResource<T, M = undefined> {
  id: string;
  type: string;
  attributes: T;
  meta: M;
}
