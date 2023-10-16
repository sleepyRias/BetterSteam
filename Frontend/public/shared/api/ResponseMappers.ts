/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/ban-ts-comment */
import { AxiosResponse } from "axios";
import { ArrayElement } from "../ArrayElement";
import { JsonApiResponse } from "./JsonApiResponse";
import { JsonApiResource } from "./JsonApiResource";
import * as HttpStatus from "http-status-codes";

export function mapResponse<T, M, R, RM>(
  response: AxiosResponse<JsonApiResponse<T, M, RM>>,
  mapper: (
    resource: JsonApiResource<T extends any[] ? ArrayElement<T> : T, M>
  ) => R
): T extends any[] ? R[] : R {
  if (response.status === HttpStatus.StatusCodes.NO_CONTENT) {
    // @ts-ignore
    return;
  } else {
    const data = response.data.data;
    // @ts-ignore
    return Array.isArray(data) ? data.map(mapper) : mapper(data);
  }
}

export function mapResponseAxios<T>(response: AxiosResponse<T>): T {
  if (response.status === HttpStatus.StatusCodes.NO_CONTENT) {
    // @ts-ignore
    return;
  } else {
    return response.data;
  }
}
