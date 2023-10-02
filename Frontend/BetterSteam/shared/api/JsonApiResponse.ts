import { ArrayElement } from "../ArrayElement";
import { JsonApiResource } from "./JsonApiResource";
export interface JsonApiResponse<T, M = undefined, RM = undefined> {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  data: T extends any[]
    ? JsonApiResource<ArrayElement<T>, M>[]
    : JsonApiResource<T, M>;
  meta: RM;
}
