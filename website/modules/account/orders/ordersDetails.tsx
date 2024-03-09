import { OrderType } from "../../../services/types/types";
import { UserCircleIcon } from "@heroicons/react/24/outline";
import { TruckIcon } from "@heroicons/react/24/outline";

export default function OrderDetails({ orderDetails }: { orderDetails: OrderType | undefined }) {
  const orderLine = orderDetails?.clientOrderLines;

  return (
    <div className="py-14 px-4 md:px-6 2xl:px-20 2xl:container 2xl:mx-auto">
      <div className="flex justify-start item-start space-y-2 flex-col">
        <h1 className="text-3xl lg:text-4xl font-semibold leading-7 lg:leading-9 text-gray-800">Commande</h1>
        <p className="text-base  font-medium leading-6 text-gray-600">{//@ts-ignore
           new Date(orderDetails?.date).toLocaleDateString("fr")}
        </p>
      </div>
        <div className="justify-center flex-1 max-w-6xl py-10 mx-auto bg-white rounded-2xl">
          <div className="flex flex-wrap items-center ">
            <div className="relative w-full px-4 mb-16 md:w-1/2 lg:w-1/4 lg:mb-0">
              <div className="absolute hidden top-5 lg:block left-1/2 ">
                        <span
                          className="mb-3 border-b-2 border-r border-green-600 w-72 md:block left-1/2 inset-px">
                        </span>
              </div>
              <div className="relative text-center">
                        <span
                          className="inline-flex items-center justify-center w-10 h-10 mb-8 text-lg text-gray-100 bg-green-600 rounded-full shadow-md">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                 className="w-6 h-6 bi bi-check-lg" viewBox="0 0 16 16">
                                <path
                                  d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z" />
                            </svg>
                        </span>
                <h2 className="text-lg font-medium">Payement effectué</h2>
              </div>
            </div>
            <div className="relative w-full px-4 mb-16 md:w-1/2 lg:w-1/4 lg:mb-0">
              <div className="absolute hidden top-5 lg:block left-1/2 ">
                        <span
                          className="mb-3 border-b-2 border-r border-gray-300 w-72 md:block left-1/2 inset-px">
                        </span>
              </div>
              <div className="relative text-center">
                        <span
                          className="inline-flex items-center justify-center w-10 h-10 mb-8 text-lg text-gray-100 bg-green-600 rounded-full shadow-md">
                            2
                        </span>
                <h2 className="text-lg font-medium">En attente de livraison</h2>
              </div>
            </div>
            <div className="relative w-full px-4 mb-16 md:w-1/2 lg:w-1/4 md:mb-0">
              <div className="absolute hidden top-5 lg:block left-1/2 ">
                        <span
                          className="mb-3 border-b-2 border-r border-gray-300 w-72 md:block left-1/2 inset-px">
                        </span>
              </div>
              <div className="relative text-center">
                        <span
                          className="inline-flex items-center justify-center w-10 h-10 mb-8 text-lg text-gray-700 bg-gray-200 rounded-full shadow-md">
                            3
                        </span>
                <h2 className="text-lg font-medium">En livraison</h2>
              </div>
            </div>
            <div className="relative w-full px-4 md:w-1/2 lg:w-1/4">
              <div className="relative text-center">
                        <span
                          className="inline-flex items-center justify-center w-10 h-10 mb-8 text-gray-700 bg-gray-200 rounded-full shadow-md">
                            4
                        </span>
                <h2 className="text-lg font-medium">Livrée</h2>
              </div>
            </div>
          </div>
        </div>
      <div
        className="mt-2 flex flex-col xl:flex-row jusitfy-center items-stretch w-full xl:space-x-8 space-y-4 md:space-y-6 xl:space-y-0">
        <div className="flex flex-col justify-start items-start w-full space-y-4 md:space-y-6 xl:space-y-8">
          <div className="flex flex-col justify-start items-start  bg-gray-50 px-4 py-4 md:py-6 md:p-6 xl:p-8 w-full">
            <p className="text-lg md:text-xl font-semibold leading-6 xl:leading-5 text-gray-800">
              Détails des produits
            </p>
            {orderLine?.map((line) => (
              <div key={line.id}
                className="mt-4 md:mt-6 flex flex-col md:flex-row justify-start items-start md:items-center md:space-x-6 xl:space-x-8 w-full">
                <div className="pb-4 md:pb-8 w-full md:w-40">
                  <img className="w-full hidden md:block" src={line.product.image} alt="" />
                </div>
                <div
                  className="border-b border-gray-200 md:flex-row flex-col flex justify-between items-start w-full pb-8 space-y-4 md:space-y-0">
                  <div className="w-full flex flex-col justify-start items-start space-y-8">
                    <h3 className="text-xl xl:text-2xl font-semibold leading-6 text-gray-800">
                      {line.product.name}</h3>
                  </div>
                  <div className="flex justify-between space-x-8 items-start w-full">
                    <p className="text-base xl:text-lg leading-6 text-gray-800">{line.quantity}</p>
                    <p className="text-base xl:text-lg font-semibold leading-6 text-gray-800">{line.price}€</p>
                  </div>
                </div>
              </div>
            ))}
          </div>
          <div
            className="flex justify-center flex-col md:flex-row items-stretch w-full space-y-4 md:space-y-0 md:space-x-6 xl:space-x-8">
            <div className="flex flex-col px-4 py-6 md:p-6 xl:p-8 w-full bg-gray-50 space-y-6">
            <h3 className="text-xl  font-semibold leading-5 text-gray-800">Sommaire</h3>
              <div className="flex justify-center items-center w-full space-y-4 flex-col border-gray-200 border-b pb-4">
                <div className="flex justify-between w-full">
                  <p className="text-base  leading-4 text-gray-800">Total</p>
                  <p className="text-base leading-4 text-gray-600">{orderDetails?.price} €</p>
                </div>
                <div className="flex justify-between items-center w-full">
                  <p className="text-base  leading-4 text-gray-800">Livraison</p>
                  <p className="text-base leading-4 text-gray-600">Offerte</p>
                </div>
              </div>
              <div className="flex justify-between items-center w-full">
                <p className="text-base font-semibold leading-4 text-gray-800">Total</p>
                <p className="text-base font-semibold leading-4 text-gray-600">{orderDetails?.price} €</p>
              </div>
            </div>
          </div>
        </div>
        <div
          className="bg-gray-50 w-full xl:w-96 flex justify-between items-center md:items-start px-4 py-6 md:p-6 xl:p-8 flex-col">
          <h3 className="text-xl font-semibold leading-5 text-gray-800">Client</h3>
          <div
            className="flex flex-col md:flex-row xl:flex-col justify-start items-stretch h-full w-full md:space-x-6 lg:space-x-8 xl:space-x-0">
            <div className="flex flex-col justify-start items-start flex-shrink-0">
              <div
                className="flex justify-center w-full md:justify-start items-center space-x-4 py-8 border-b border-gray-200">
                <UserCircleIcon className="h-[24px] w-[24px]"/>
                <div className="flex justify-start items-start flex-col space-y-2">
                  <p className="text-base font-semibold leading-4 text-left text-gray-800">{orderDetails?.client.firstName}{" "}{orderDetails?.client.lastName}</p>
                </div>
              </div>

              <div
                className="flex justify-center text-gray-800 md:justify-start items-center space-x-4 py-4 border-b border-gray-200 w-full">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path
                    d="M19 5H5C3.89543 5 3 5.89543 3 7V17C3 18.1046 3.89543 19 5 19H19C20.1046 19 21 18.1046 21 17V7C21 5.89543 20.1046 5 19 5Z"
                    stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" />
                  <path d="M3 7L12 13L21 7" stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" />
                </svg>
                <p className="cursor-pointer text-sm leading-5 ">{orderDetails?.client.email}</p>
              </div>
            </div>
            <div className="flex justify-between xl:h-full items-stretch w-full flex-col mt-6 md:mt-0">
              <div
                className="flex justify-center md:justify-start xl:flex-col flex-col md:space-x-6 lg:space-x-8 xl:space-x-0 space-y-4 xl:space-y-12 md:space-y-0 md:flex-row items-center md:items-start">
                <div
                  className="flex justify-center md:justify-start items-center md:items-start flex-col xl:mt-8">
                   <span className="flex mb-4">
                  <TruckIcon className="h-[24px] w-[24px]" />
                  <p
                    className="text-base flex items-center ml-[16px] font-semibold leading-4 text-center md:text-left text-gray-800">Adresse de
                    livraison</p>
                   </span>
                  <p
                    className="w-48 lg:w-full xl:w-48 text-center md:text-left text-sm leading-5 font-bold text-gray-600">{orderDetails?.client.address.name}</p>
                  <p
                    className="w-48 lg:w-full xl:w-48 text-center md:text-left text-sm leading-5 text-gray-600">{orderDetails?.client.address.street}</p>
                  <p
                    className="w-48 lg:w-full xl:w-48 text-center md:text-left text-sm leading-5 text-gray-600">{orderDetails?.client.address.number}</p>
                  <p
                    className="w-48 lg:w-full xl:w-48 text-center md:text-left text-sm leading-5 text-gray-600">{orderDetails?.client.address.city}</p>
                  <p
                    className="w-48 lg:w-full xl:w-48 text-center md:text-left text-sm leading-5 text-gray-600">{orderDetails?.client.address.country}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}