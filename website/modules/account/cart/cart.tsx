"use client"
import { useAccount } from "../../../services/api/user/useAccount";
import EmptyCart from "./emptyCart";
import React, { useEffect, useState } from "react";
import {getOrderClient, deleteOrderClientLine, putOrderClient } from "../../../services/api/products/cart";
import {OrderLineType } from "../../../services/types/types";
import Loading from "../../extras/loading";
import { useRouter } from "next/navigation";
import Button from "../../extras/button";
import LoadingButton from "../../extras/buttonLoading";

export default function Cart() {
  const router = useRouter();
  const { account} = useAccount();
  const [ordersline, setOrdersline] = useState<OrderLineType[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isButtonLoading, setIsButtonLoading] = useState(false);

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
      setOrdersline(ordersline.filter((orderline) => orderline.id !== orderId));
      console.log(ordersline)
      const event = new CustomEvent('cartUpdated');
      setTimeout(() => {
        const event = new CustomEvent('cartUpdated');
        window.dispatchEvent(event);
      }, 1000);
      if (ordersline.length < 1) {
        window.location.reload();
      }
    } catch (error) {
        console.error('Error deleting order line:', error);
    }
};

  const total = ordersline.reduce((acc, orderline) => acc + orderline.price, 0);

  if (isLoading) return <Loading/>

  return (
    <>
      {ordersline.length === 0 ?
        <EmptyCart account={account ? account : undefined}/>
        : (
          <div className="h-scren bg-gray-100 pt-10 pb-10">
            <h1 className="mb-10 text-center text-2xl font-bold">Panier</h1>
            <div className="mx-auto max-w-5xl flex-col-reverse md:flex-col justify-start px-6 flex md:space-x-6 xl:px-0">
              <div className="rounded-lg md:w-2/3 overflow-y-auto">
                {ordersline.map((orderLine,index) => (
                <div key={index} className="justify-between mb-6 rounded-lg overflow-y-auto bg-white p-6 shadow-md sm:flex sm:justify-start">
                  <img
                    src={orderLine.product.image}
                    alt="product-image" className="w-full rounded-lg sm:w-40" />
                  <div className="sm:ml-4 sm:flex sm:w-full sm:justify-between">
                    <div className="mt-5 sm:mt-0">
                      <h2 className="text-lg font-bold text-gray-900">{orderLine.product.name}</h2>
                      <p
                        className="mt-1 text-xs text-gray-700">{new Date(orderLine.product.dateProduction).toLocaleDateString("fr-FR", {
                        year: "numeric",
                      })}</p>
                      <p className="mt-1 text-xs text-gray-700">Quantité: {orderLine.quantity}</p>
                    </div>
                    <div className="mt-4 flex justify-between sm:space-y-6 sm:mt-0 sm:block sm:space-x-6">
                      <div className="flex items-center space-x-4">
                      <p className="text-sm">{orderLine.price} €</p>
                        <svg onClick={() => handleDeleteOrderLine(orderLine.id)}
                          xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5"
                             stroke="currentColor" className="h-5 w-5 cursor-pointer duration-150 hover:text-red-500">
                          <path strokeLinecap="round" strokeLinejoin="round" d="M6 18L18 6M6 6l12 12" />
                        </svg>
                      </div>
                    </div>
                  </div>
                </div>
                ),)}
              </div>
              <div className="mt-6 md:fixed right-8 rounded-lg border bg-white p-6 shadow-md md:mt-0 mb-4 md:w-1/3">
                <div className="mb-2 flex justify-between">
                  <p className="text-gray-700">Total des produits</p>
                  <p className="text-gray-700">{total} €</p>
                </div>
                <div className="flex justify-between">
                  <p className="text-gray-700">Livraison</p>
                  <p className="text-gray-700">Offerte</p>
                </div>
                <hr className="my-4" />
                <div className="flex justify-between">
                  <p className="text-lg font-bold">Total</p>
                  <div className="flex flex-col items-end">
                    <p className="mb-1 text-lg font-bold">{total} €</p>
                    <p className="text-sm text-gray-700">Taxes incluses</p>
                  </div>
                </div>
                {isButtonLoading ? <LoadingButton className="mt-6 w-full rounded-md bg-blue-500 py-1.5 font-medium text-blue-50 hover:bg-blue-600" /> :
                <button
                  onClick={(() => {
                    setIsLoading(true);
                    router.push('/account/cart/delivery');
                  })}
                  className="mt-6 w-full rounded-md bg-blue-500 py-1.5 font-medium text-blue-50 hover:bg-blue-600">
                  Commander
                </button>
                }
              </div>
            </div>
          </div>
        )}
    </>
  );
}
