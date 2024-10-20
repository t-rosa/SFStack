import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_public/login")({
    component: () => <div>Hello /_public/login!</div>,
});
