import { create } from "zustand";
import { combine, persist } from "zustand/middleware";
import { Account } from "../../types/types";

export const useAccountStore = create(
  persist(
    combine(
    {
      account: null as undefined | null | Account
    },
    (set) => ({
      setAccount: (account: null | Account) => set({ account })
    })
  ),
    {name : "account"}
)
);