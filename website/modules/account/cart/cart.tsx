"use client"
import { useAccount } from "../../../services/api/user/useAccount";
import EmptyCart from "./emptyCart";
import React, { useEffect, useState } from "react";
import {getOrderClient, deleteOrderClientLine } from "../../../services/api/products/cart";
import {OrderLineType } from "../../../services/types/types";
import Loading from "../../extras/loading";

export default function Cart() {
  const { account} = useAccount();
  const [orders, setOrders] = useState<OrderLineType[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const ordersDate = await getOrderClient(account?.id);
        setOrders(ordersDate);
        setIsLoading(false);
      } catch (error) {
        console.error('Error during product retrieval:', error);
        setIsLoading(false);
      }
    };
    fetchData();
  }, []);

  const handleDeleteOrderLine = async (orderId: number) => {
    try {
        await deleteOrderClientLine(orderId);
        window.location.reload(); // Recharge la page après la suppression
    } catch (error) {
        console.error('Error deleting order line:', error);
    }
};

  const total = orders.reduce((acc, product) => acc + product.price, 0);

  if (isLoading) return <Loading/>

  return (
    <>
      {orders.length === 0 ?
        <EmptyCart account={account ? account : undefined}/>
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
                <span>
                <button onClick={() => handleDeleteOrderLine(order.id)} className="w-full bg-gray-900 text-white py-2 px-4 rounded-full font-bold hover:bg-gray-800">Delete</button>
                </span>
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
