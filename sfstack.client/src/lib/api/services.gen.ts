// This file is auto-generated by @hey-api/openapi-ts

import { createClient, createConfig, type Options } from "@hey-api/client-fetch";
import {
    type GetWeatherForecastError,
    type GetWeatherForecastResponse,
    GetWeatherForecastResponseTransformer,
} from "./types.gen";

export const client = createClient(createConfig());

export const getWeatherForecast = <ThrowOnError extends boolean = false>(
    options?: Options<unknown, ThrowOnError>,
) => {
    return (options?.client ?? client).get<
        GetWeatherForecastResponse,
        GetWeatherForecastError,
        ThrowOnError
    >({
        ...options,
        url: "/WeatherForecast",
        responseTransformer: GetWeatherForecastResponseTransformer,
    });
};
