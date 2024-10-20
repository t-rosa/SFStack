import { create } from "zustand";
import { devtools, persist } from "zustand/middleware";

interface State {
    user: { isAuthenticated: boolean };
}

interface Actions {
    setUser: () => void;
    resetUser: () => void;
}

const initialState: State = {
    user: {
        isAuthenticated: false,
    },
};

export const useAuthenticationStore = create<State & Actions>()(
    devtools(
        persist(
            (set) => ({
                ...initialState,
                setUser: () => set({ user: { isAuthenticated: true } }),
                resetUser: () => set(initialState),
            }),
            { name: "authenticationStore" },
        ),
    ),
);
