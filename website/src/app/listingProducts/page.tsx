"use client"
import Footer from "../../../modules/layout/footer";
import Header from "../../../modules/layout/header";
import ProductListing from "../../../modules/products/listing/productListing";
import "../globals.css";

export default function ListingProducts() {
  const id = 1;
  return (
    <>
    <Header/>
    <ProductListing/>
      <a href={`listingProducts/product/${id}`}>
        test
      </a>
    </>
  )
}