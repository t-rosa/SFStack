import { ThemeProvider } from "./theme-provider";
import { type NavigateOptions, type ToOptions, useRouter } from "@tanstack/react-router";
import { RouterProvider as AriaRouterProvider } from "react-aria-components";

declare module "react-aria-components" {
    interface RouterConfig {
        href: ToOptions["to"];
        routerOptions: Omit<NavigateOptions, keyof ToOptions>;
    }
}

export function Providers(props: React.PropsWithChildren) {
    const router = useRouter();

    return (
        <AriaRouterProvider
            navigate={(to, options) => router.navigate({ to, ...options })}
            useHref={(to) => router.buildLocation({ to }).href}
        >
            <ThemeProvider>{props.children}</ThemeProvider>
        </AriaRouterProvider>
    );
}
