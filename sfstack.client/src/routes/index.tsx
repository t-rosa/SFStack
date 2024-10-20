import { ThemeSwitcher } from "@/components/theme-switcher";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/")({
    component: () => (
        <div>
            <ThemeSwitcher />
        </div>
    ),
});
