import { OrderStatus, OrderType } from "../../../services/types/types";
import Arrow from "../../svg/arrowRight.svg";
import React from "react";
import { useRouter } from "next/navigation";

export default function Orders({ orders }: { orders: OrderType[] }) {
  const router = useRouter();

  function verificationOrder(orders: OrderType[]): boolean {
    for (const order of orders) {
      if (order.orderStatus !== OrderStatus.ENCOURSDEVALIDATION) {
        return true;
      }
    }
    return false;
  }

  const isOrder = verificationOrder(orders);
return (
<div className="m-5">
  {!isOrder ?
    <div className="text-2xl font-semibold mb-3">
      <p>Dommage! Vous n&apos;avez aucune commande validée</p>
    </div>
    :
  <ul className="flex flex-wrap">
    {orders.map((order) => (
      order.orderStatus === 4 ? null :
      <li key={order.id} onClick={(() => {router.push(`/account/order/${order.id}`)})}
          className="transition duration-300 ease-in-out hover:scale-105 sm:w-1/2 md:w-1/3 lg:w-1/4 relative m-5 flex w-full cursor-pointer max-w-xs flex-col overflow-hidden rounded-lg border border-gray-100 bg-white shadow-md">
        <div className="relative flex flex-col mt-6 text-gray-700 bg-white shadow-md bg-clip-border rounded-xl w-96">
            <h5
              className="block mb-2 ml-5 font-sans text-xl antialiased font-semibold leading-snug tracking-normal text-blue-gray-900">
             Commande du {new Date(order.date).toLocaleDateString()}
            </h5>
              <p className="block font-sans text-base ml-5 m-2 antialiased font-light leading-relaxed text-inherit">
                {order.price} €
              </p>
                <a className="inline-block">
                    <button
                      className="flex items-center gap-2 m-5 px-4 py-2 font-sans text-xs font-bold text-center text-gray-900 uppercase align-middle transition-all rounded-lg select-none disabled:opacity-50 disabled:shadow-none disabled:pointer-events-none hover:bg-gray-900/10 active:bg-gray-900/20"
                      type="button">
                      Afficher les détails
                      <Arrow />
                    </button>
                </a>
        </div>
      </li>
    ))}
  </ul>
  }
</div>
)
}