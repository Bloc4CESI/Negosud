"use client"
import { useAccount } from "../../../services/api/user/useAccount";
import EmptyCart from "./emptyCart";
import React, { useEffect, useState } from "react";
import {getOrderClient, deleteOrderClientLine, putOrderClient } from "../../../services/api/products/cart";
import {OrderLineType } from "../../../services/types/types";
import Loading from "../../extras/loading";

export default function Cart() {
  const { account} = useAccount();
  const [ordersline, setOrdersline] = useState<OrderLineType[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const ordersDate = await getOrderClient(account?.id);
        setOrdersline(ordersDate);
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

const handleModifyOrder = async () => {
  try {
    const data = {
      id:ordersline[0]?.clientOrder?.id,
      date: new Date(),
      price: total,
      orderStatus: "VALIDE"
    } 
    console.log(data)
    await putOrderClient(data);
  } catch (error) {
    console.error('Error deleting order line:', error);
}
  
}

  const total = ordersline.reduce((acc, orderline) => acc + orderline.price, 0);

  if (isLoading) return <Loading/>

  return (
    <>
      {ordersline.length === 0 ?
        <EmptyCart account={account ? account : undefined}/>
        : (
      <div className="container mx-auto mt-8 flex">
        <div className="w-2/3 pr-8">
          <h2 className="text-2xl font-bold mb-4">Panier</h2>
          <ul>
            {ordersline.map((orderline, index) => (
              <li
                key={orderline.id}
                className={`flex justify-between my-4 ${
                  index < ordersline.length ? "bg-gray-200 rounded-full py-2 px-4" : ""
                }`}
              >
                <span>{orderline.product.name}</span>
                <span>{orderline.quantity}</span>
                <span>{orderline.price} €</span>
                <span>
                <button onClick={() => handleDeleteOrderLine(orderline.id)} className="w-full bg-gray-900 text-white py-2 px-4 rounded-full font-bold hover:bg-gray-800">Delete</button>
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
              <button onClick={handleModifyOrder} className="mt-4 bg-gray-900 text-white px-4 py-2 rounded-full">
                Commander
              </button>
          </div>
        </div>
      </div>
)}
    </>
  );
}
