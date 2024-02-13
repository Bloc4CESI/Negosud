"use client"
import Footer from "../../../../../modules/layout/footer";
import Header from "../../../../../modules/layout/header";
import FormProduct from "../../../../../modules/products/product/formProduct";
import "../../../globals.css";

export default function Product({ params }: { params: { id: number }}) {
  const { id } = params;
  return (
    <>
    <Header style={undefined}/>
    <FormProduct id={id}/>
    <Footer/>
    </>
  )
}