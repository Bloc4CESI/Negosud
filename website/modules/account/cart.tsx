"use client"
import React from "react";
import "../globals.css";
import { useAccount } from "../../services/api/user/useAccount";
import EmptyCart from "./emptyCart";

export default function Cart() {
  const { account} = useAccount();
  const products: any[] = [];

  const total = products.reduce((acc, product) => acc + product.price, 0);

  return (
    <>
      {products.length === 0 ?
        <EmptyCart account={account}/>
        : (
      <div className="container mx-auto mt-8 flex">
        <div className="w-2/3 pr-8">
          <h2 className="text-2xl font-bold mb-4">Panier</h2>
          <ul>
            {products.map((product, index) => (
              <li
                key={product.id}
                className={`flex justify-between my-4 ${
                  index < products.length ? "bg-gray-200 rounded-full py-2 px-4" : ""
                }`}
              >
                <span>{product.name}</span>
                <span>{product.price} €</span>
              </li>
            ))}
          </ul>
        </div>
        <div className="w-1/3">
          <div className="bg-gray-200 p-4 rounded-lg">
            <h2 className="text-xl font-bold mb-4">Total du Panier</h2>
            <p className="flex justify-between">
              <span>Total :</span>
              <span>{total} €</span>
            </p>
            <button className="mt-4 bg-gray-900 text-white px-4 py-2 rounded-full">
              Commander
            </button>
          </div>
        </div>
      </div>
)}
    </>
  );
}
