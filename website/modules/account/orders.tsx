import { OrderType } from "../../services/types/types";
import Arrow from "../svg/arrowRight.svg";

export default function Orders({ orders }: { orders: OrderType[] }) {
  console.log(orders);
return (
  <>
    {orders.map((order) => (
      <div key={order.id}
           className="relative flex flex-col mt-6 text-gray-700 bg-white shadow-md bg-clip-border rounded-xl w-96">
        <h5
          className="block mb-2 font-sans text-xl antialiased font-semibold leading-snug tracking-normal text-blue-gray-900">
         Commande du {order.date}
        </h5>
        <p className="block font-sans text-base antialiased font-light leading-relaxed text-inherit">
          {order.price} €
        </p>
        <a href="#" className="inline-block">
          <button
            className="flex items-center gap-2 px-4 py-2 font-sans text-xs font-bold text-center text-gray-900 uppercase align-middle transition-all rounded-lg select-none disabled:opacity-50 disabled:shadow-none disabled:pointer-events-none hover:bg-gray-900/10 active:bg-gray-900/20"
            type="button">
            Afficher les détails
            <Arrow />
          </button>
        </a>
      </div>
    ))}
  </>
)
}