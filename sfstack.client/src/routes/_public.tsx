import { useAuthenticationStore } from "@/modules/authentication/store";
import { createFileRoute, Navigate, Outlet } from "@tanstack/react-router";

export const Route = createFileRoute("/_public")({
    component: PublicLayout,
});

function PublicLayout() {
    const auth = useAuthenticationStore();

    if (auth.user.isAuthenticated) {
        return <Navigate to="/dashboard" />;
    }

    return <Outlet />;
}
