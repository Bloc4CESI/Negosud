"use client";
import User from "../../../modules/account/user";
import { useAccount } from "../../../services/api/user/useAccount";
import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import { useRouter } from "next/navigation";
import "../globals.css";
import { useEffect, useState } from "react";
import { useAuth } from "../../../services/api/user/useAuth";

export default function Account() {
  debugger;
  const { account} = useAccount();
  const { authenticate } = useAuth();
  const router = useRouter();
  const [connected, setConnected] = useState(false);
  
  useEffect( () => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
       // @ts-ignore
      authenticate(account?.email);
    }
  }, []);

  return (
    <>
      <Header />
      <User userData={account} />
      <Footer />
    </>
  );
};