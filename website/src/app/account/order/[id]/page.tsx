"use client";
import Header from "../../../../../modules/layout/header";
import Footer from "../../../../../modules/layout/footer";
import OrderDetails from "../../../../../modules/account/orders/ordersDetails";
import { useEffect, useState } from "react";
import { getOrderById } from "../../../../../services/api/products/cart";
import "../../../globals.css";
import { OrderType } from "../../../../../services/types/types";
import Loading from "../../../../../modules/extras/loading";

export default function Page({ params }: { params: { id: number }}) {
  const id = params;
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [orderDetails, setOrderDetails] = useState<OrderType>();

  useEffect( () => {
     getOrderById(id.id).then(r => {
       setOrderDetails(r);
       setIsLoading(false);
     })
  },[])

  return (
    <>
    <Header/>
      {isLoading? <Loading/> : <OrderDetails orderDetails={orderDetails} />}
    <Footer/>
    </>
  )
}