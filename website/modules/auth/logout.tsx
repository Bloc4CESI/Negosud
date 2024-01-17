import { useLogout } from "../../services/api/user/userService";

export default function Logout() {
  return (
    <button onClick={useLogout}>
      Se déconnecter
    </button>
  )
}