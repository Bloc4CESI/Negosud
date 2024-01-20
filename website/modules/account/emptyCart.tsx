import React from "react";
import { Account } from "../../services/types/types";
export default function EmptyCart({ account }: { account: Account }) {

  return (
    <div className="max-w-screen-xl mx-auto p-4">
      <div className="my-8">
        <h1 className="text-4xl font-semibold mb-4">Votre panier est vide</h1>
        <p>Allons régler cela {account.firstName}</p>
        <a href={"/listingProducts"} className="text-lg">
          Produit
        </a>
      </div>
    </div>
  )
}