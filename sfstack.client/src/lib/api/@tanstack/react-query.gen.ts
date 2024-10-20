// This file is auto-generated by @hey-api/openapi-ts

import type { Options } from "@hey-api/client-fetch";
import { queryOptions } from "@tanstack/react-query";
import { client, getWeatherForecast } from "../services.gen";

type QueryKey<TOptions extends Options> = [
    Pick<TOptions, "baseUrl" | "body" | "headers" | "path" | "query"> & {
        _id: string;
        _infinite?: boolean;
    },
];

const createQueryKey = <TOptions extends Options>(
    id: string,
    options?: TOptions,
    infinite?: boolean,
): QueryKey<TOptions>[0] => {
    const params: QueryKey<TOptions>[0] = {
        _id: id,
        baseUrl: (options?.client ?? client).getConfig().baseUrl,
    } as QueryKey<TOptions>[0];
    if (infinite) {
        params._infinite = infinite;
    }
    if (options?.body) {
        params.body = options.body;
    }
    if (options?.headers) {
        params.headers = options.headers;
    }
    if (options?.path) {
        params.path = options.path;
    }
    if (options?.query) {
        params.query = options.query;
    }
    return params;
};

export const getWeatherForecastQueryKey = (options?: Options) => [
    createQueryKey("getWeatherForecast", options),
];

export const getWeatherForecastOptions = (options?: Options) => {
    return queryOptions({
        queryFn: async ({ queryKey }) => {
            const { data } = await getWeatherForecast({
                ...options,
                ...queryKey[0],
                throwOnError: true,
            });
            return data;
        },
        queryKey: getWeatherForecastQueryKey(options),
    });
};
