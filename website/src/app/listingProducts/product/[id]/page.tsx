"use client"
import Footer from "../../../../../modules/layout/footer";
import Header from "../../../../../modules/layout/header";
import FormProduct from "../../../../../modules/products/product/formProduct";
import "../../../globals.css";
import { useState } from "react";

export default function Product({ params }: { params: { id: number }}) {
  const { id } = params;

  return (
    <>
    <Header/>
    <FormProduct id={id}/>
    <Footer/>
    </>
  )
}