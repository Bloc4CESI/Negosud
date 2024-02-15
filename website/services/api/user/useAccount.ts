import { useAuth } from "./useAuth";
import { useRouter } from "next/navigation";
import toast from "react-hot-toast";

export function useAccount() {
  const {account} = useAuth();
  return {
    account,
  }
}