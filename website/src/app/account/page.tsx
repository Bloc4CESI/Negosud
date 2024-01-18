"use client";
import User from "../../../modules/auth/user";
import { user } from "../../../services/api/user/userService";
import { useAuth } from "../../../services/api/user/useAuth";
import { useEffect } from "react";
import { useAccount } from "../../../services/api/user/useAccount";
import Logout from "../../../modules/auth/logout";

export default function Account() {
  const { account } = useAccount();
  console.log(account);
  return (
    <>
      <User userData={account} />
      <Logout />
    </>
  );
};