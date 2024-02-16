"use client";
import Header from "modules/layout/header";
import Footer from "modules/layout/footer";
import DeliveryAddress from "modules/account/cart/deliveryAddress";
import "../../../globals.css";

export default function New() {
  return (
    <>
      <Header />
      <DeliveryAddress/>
      <Footer />
    </>
  )
}