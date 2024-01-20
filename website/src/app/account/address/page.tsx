"use client"
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Header from "../../../../modules/layout/header";
import Footer from "../../../../modules/layout/footer";
import { useAccount } from "../../../../services/api/user/useAccount";
import Address from "../../../../modules/account/adress";
import '../../globals.css';
import { getAddresses } from "../../../../services/api/user/userService";
import { AddressType } from "../../../../services/types/types";
import Loading from "../../../../modules/extras/loading";

export default function AddressPage() {
  const { account} = useAccount();
  const [loading, setLoading] = useState(true);
  const [connected, setConnected] = useState(false);
  const [addresses, setAddresses] = useState<AddressType[]>([]);

  const router = useRouter();
  useEffect(() => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
      getAddresses(account.addressId).then((addresses) => {
      setAddresses([addresses]);
      setLoading(false);
      });
    }
  }, []);

  return (
    <>
      <Header/>
      {loading ? (<Loading/>) : (<Address addresses={addresses}/>)}
      <Footer/>
    </>
  )
}