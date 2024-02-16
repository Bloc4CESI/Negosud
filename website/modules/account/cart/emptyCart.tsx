import React from "react";
import { Account } from "../../../services/types/types";
import { ArrowRightIcon } from "@heroicons/react/24/outline";
export default function EmptyCart({ account }: { account?: Account }) {

  return (
    <div className="max-w-screen-xl h-screen mx-auto p-4">
      <div className="my-8">
        <h1 className="text-4xl font-semibold mb-4">Votre panier est vide</h1>
        <p>Allons régler cela {account?.lastName}</p>
        <a href={"/listingProducts"} className="text-lg items-center flex">
         <p className="mr-2">
          Nos Produits
         </p>
          <ArrowRightIcon className="h-5 w-5"/>
        </a>
      </div>
    </div>
  )
}