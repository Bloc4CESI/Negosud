"use client";
import User from "../../../modules/account/user";
import { useAccount } from "../../../services/api/user/useAccount";
import Logout from "../../../modules/extras/button";
import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import { useRouter } from "next/navigation";
import "../globals.css";
import { useEffect, useState } from "react";
import Address from "../../../modules/account/adress";
import { getAddresses } from "../../../services/api/user/userService";
import { AddressType } from "../../../services/types/types";

export default function Account() {
  const { account} = useAccount();
  const router = useRouter();
  const [connected, setConnected] = useState(false);

  useEffect(() => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
    }
  }, []);

  return (
    <>
      <Header style={undefined} />
      <User userData={account} />
      <Footer/>
    </>
  );
};