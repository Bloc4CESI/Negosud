"use client"
import { useAccount } from "../../services/api/user/useAccount";
import EmptyCart from "./emptyCart";
import React, { useEffect, useState } from "react";
import {getOrderClient } from "../../services/api/products/cart";
import {OrderLineType } from "../../services/types/types";

export default function Cart() {
  const { account} = useAccount();
  const [orders, setOrders] = useState<OrderLineType[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const ordersDate = await getOrderClient(account.id);
        setOrders(ordersDate);
        setIsLoading(false);
      } catch (error) {
        console.error('Error during product retrieval:', error);
        setIsLoading(false);
      }
    };
    fetchData();
  }, []);

  const total = orders.reduce((acc, product) => acc + product.price, 0);
console.log('######' + orders);
  return (
    <>
      {orders.length === 0 ?
        <EmptyCart account={account}/>
        : (
      <div className="container mx-auto mt-8 flex">
        <div className="w-2/3 pr-8">
          <h2 className="text-2xl font-bold mb-4">Panier</h2>
          <ul>
            {orders.map((order, index) => (
              <li
                key={order.id}
                className={`flex justify-between my-4 ${
                  index < orders.length ? "bg-gray-200 rounded-full py-2 px-4" : ""
                }`}
              >
                <span>{order.product.name}</span>
                <span>{order.quantity}</span>
                <span>{order.price} €</span>
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
