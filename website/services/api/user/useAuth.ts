import { Account } from "../../types/types";
import { useAccountStore } from "./store";
import { useCallback } from "react";
import { apiFetch } from "./api";
import { API_BASE_URL } from "./userService";

export enum AuthStatus {
  Unknown,
  Authenticated,
  Guest

}
export function useAuth() {
  const {account, setAccount} = useAccountStore()
  let status;
  switch (account) {
    case null:
      status = AuthStatus.Guest
      break;
    case undefined:
      status = AuthStatus.Unknown
      break;
    default:
      status = AuthStatus.Authenticated
      break;
  }

  const authenticate = useCallback(async (encodedEmail: string) => {
    const r = await fetch(`${API_BASE_URL}/Client/GetByEmail/${encodedEmail}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    })
      const user = await r.json()
      setAccount(user)
  }, []);

  return {
    account,
    status,
    authenticate,
  };
  }