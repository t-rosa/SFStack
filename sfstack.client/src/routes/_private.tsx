import { useAuthenticationStore } from "@/modules/authentication/store";
import { createFileRoute, Navigate, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/_private")({
    component: PrivateLayout,
});

function PrivateLayout() {
    const auth = useAuthenticationStore();

    if (auth.user.isAuthenticated) {
        return <Outlet />;
    }

    return <Navigate to="/login" />;
}
