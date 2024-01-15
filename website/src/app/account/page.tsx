"use client";
import User from "../../../modules/auth/user";
import { user } from "../../../services/api/user/userService";

export default function Account() {
  return (
    <User userData={user}/>
  );
};