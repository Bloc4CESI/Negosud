import { useAuth } from "./useAuth";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";

export function useAccount() {
  const router = useRouter();
  const {account} = useAuth()
debugger;
  if (!account) {
    router.push('/login');
  }
  return {
    account,
  }
}