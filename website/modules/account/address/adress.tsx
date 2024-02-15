import { AddressType } from "../../../services/types/types";
import Arrow from "../../svg/arrowRight.svg";


export default function Address({ addresses }: {addresses: AddressType[]}) {

return (
  <>
  {addresses.map((address) => (
  <div key={address.id} className="relative flex flex-col mt-6 text-gray-700 bg-white shadow-md bg-clip-border rounded-xl w-96">
    <div className="p-6">
      <h5
        className="block mb-2 font-sans text-xl antialiased font-semibold leading-snug tracking-normal text-blue-gray-900">
        {address.name}
      </h5>
      <p className="block font-sans text-base antialiased font-light leading-relaxed text-inherit">
        {address.street}
      </p>
      <p className="block font-sans text-base antialiased font-light leading-relaxed text-inherit">
        {address.number} {address.city}
      </p>
      <p className="block font-sans text-base antialiased font-light leading-relaxed text-inherit">
        {address.country}
      </p>
    </div>
    <div className="p-6 pt-0">
      <a href="#" className="inline-block">
        <button
          className="flex items-center gap-2 px-4 py-2 font-sans text-xs font-bold text-center text-gray-900 uppercase align-middle transition-all rounded-lg select-none disabled:opacity-50 disabled:shadow-none disabled:pointer-events-none hover:bg-gray-900/10 active:bg-gray-900/20"
          type="button">
          Modifier cette adresse
         <Arrow/>
        </button>
      </a>
    </div>
  </div>
  ))}
  </>
);
}