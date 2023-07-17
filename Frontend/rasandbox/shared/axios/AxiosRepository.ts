/* eslint-disable */
import { AxiosRequestConfig, AxiosInstance, AxiosResponse } from "axios";
import { mapResponseAxios } from "../api/responseMappers";

type ResponseMapper<R> = (response: AxiosResponse) => R;

export class AxiosRepository {
  constructor(protected axios: AxiosInstance) {}

  protected async sendGet<T>(
    url: string,
    config?: AxiosRequestConfig,
    mappingFn: ResponseMapper<T> = (r) => this.transformResponse(r)
  ) {
    return mappingFn(await this.axios.get(url, config));
  }

  protected async sendHtmlGet(url: string, config?: AxiosRequestConfig) {
    return this.axios.get(url, config);
  }

  protected async sendDelete<T = void>(
    url: string,
    config?: AxiosRequestConfig,
    mappingFn: ResponseMapper<T> = (r) => this.transformResponse(r)
  ): Promise<T extends void ? void : T> {
    const response = await this.axios.delete(url, config);
    // @ts-ignore
    return response ? mappingFn(response) : Promise.resolve();
  }

  protected async sendPost<T>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig,
    mappingFn: ResponseMapper<T> = (r) => this.transformResponse(r)
  ) {
    return mappingFn(await this.axios.post(url, data, config));
  }

  protected async sendPut<T>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig,
    mappingFn: ResponseMapper<T> = (r) => this.transformResponse(r)
  ) {
    return mappingFn(await this.axios.put(url, data, config));
  }

  protected async sendPatch<T>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig,
    mappingFn: ResponseMapper<T> = (r) => this.transformResponse(r)
  ) {
    return mappingFn(await this.axios.patch(url, data, config));
  }

  protected transformResponse(response: AxiosResponse): any {
    return mapResponseAxios(response);
  }
}
