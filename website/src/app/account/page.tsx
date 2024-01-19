"use client";
import User from "../../../modules/auth/user";
import { useAccount } from "../../../services/api/user/useAccount";
import Logout from "../../../modules/auth/logout";
import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import { useRouter } from "next/navigation";
import "../globals.css";

export default function Account() {
  const { account} = useAccount();
  const router = useRouter();

  if (localStorage.getItem('connected') === null) {
    router.push('/login');
  }
  return (
    <>
      <Header />
      <User userData={account} />
      <Logout />
      <Footer/>
    </>
  );
};