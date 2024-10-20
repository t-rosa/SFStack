import { Providers } from "@/components/providers";
import { Outlet, createRootRoute } from "@tanstack/react-router";

export const Route = createRootRoute({
    component: () => (
        <Providers>
            <div>Hello "__root"!</div>
            <Outlet />
        </Providers>
    ),
});
