"use client";
import Header from "../../../../../modules/layout/header";
import Footer from "../../../../../modules/layout/footer";
import AddressForm from "../../../../../modules/account/address/addressForm";
import "../../../globals.css";

export default function New() {
  return (
    <>
      <Header />
      <AddressForm/>
      <Footer />
    </>
  )
}