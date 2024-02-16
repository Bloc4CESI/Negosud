"use client";
import User from "../../../modules/account/user";
import { useAccount } from "../../../services/api/user/useAccount";
import Logout from "../../../modules/extras/button";
import Header from "../../../modules/layout/header";
import Footer from "../../../modules/layout/footer";
import { useRouter } from "next/navigation";
import "../globals.css";
import { useCallback, useEffect, useState } from "react";
import Address from "../../../modules/account/address/adress";
import { API_BASE_URL, getAddresses } from "../../../services/api/user/userService";
import { AddressType } from "../../../services/types/types";
import { Button } from "@mui/base";
import Loading from "../../../modules/extras/loading";
import { ArrowRightIcon } from "@heroicons/react/24/outline";
import LoadingButton from "../../../modules/extras/buttonLoading";
import { useAuth } from "../../../services/api/user/useAuth";

export default function Account() {
  const { account} = useAccount();
  const { authenticate } = useAuth();
  const router = useRouter();
  const [connected, setConnected] = useState(false);
 console.log(account)
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