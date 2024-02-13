import { useAuth } from "./useAuth";
import { useRouter } from "next/navigation";

export function useAccount() {
  const router = useRouter();
  const {account} = useAuth()

  if (!account) {
    router.push('/login');
    throw new Error('Pas connecté');
  }
  return {
    account,
  }
}