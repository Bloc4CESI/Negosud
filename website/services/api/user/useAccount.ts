import { useAuth } from "./useAuth";

export function useAccount() {
  const {account} = useAuth()
  if (!account) {
    throw new Error('Pas connecté')
  }
  return {
    account,
  }
}