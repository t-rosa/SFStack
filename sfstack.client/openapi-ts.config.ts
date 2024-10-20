import { defineConfig } from "@hey-api/openapi-ts";

export default defineConfig({
    client: "@hey-api/client-fetch",
    input: "http://localhost:5000/swagger/v1/swagger.json",
    output: {
        path: "src/lib/api",
    },
    plugins: ["@tanstack/react-query"],
    types: {
        dates: "types+transform",
        enums: "javascript",
    },
});
