"use client";
import Header from "../../../../../modules/layout/header";
import Footer from "../../../../../modules/layout/footer";
import AddressForm from "../../../../../modules/account/addressForm";
import "../../../globals.css";

export default function New() {
  return (
    <>
      <Header style={undefined} />
      <div> Nouvelle Adresse</div>
      <AddressForm/>
      <Footer />
    </>
  )
}