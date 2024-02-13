"use client"
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Header from "../../../../modules/layout/header";
import Footer from "../../../../modules/layout/footer";
import '../../globals.css';
import { OrderType } from "../../../../services/types/types";
import { getOrders } from "../../../../services/api/user/userService";
import { useAccount } from "../../../../services/api/user/useAccount";
import Loading from "../../../../modules/extras/loading";
import Orders from "../../../../modules/account/orders";

export default function OrdersPage() {
  const { account} = useAccount();
  const [connected, setConnected] = useState(false);
  const router = useRouter();
  const [loading, setLoading] = useState(true);
const [orders, setOrders] = useState<OrderType[]>([]);

  useEffect(() => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
      getOrders(account.id).then((orders) => {
      setOrders(orders);
      setLoading(false);
      });
    }
  }, []);

return (
  <>
    <Header/>
    {loading ? (<Loading/>) : (<Orders orders={orders}/>) }
    <Footer/>
  </>
)
}