"use client"
import { Account } from "../../services/types/types";
import Button from "../extras/button";
import { getAddresses, Logout } from "../../services/api/user/userService";
import { useRouter } from "next/navigation";
import React, { useEffect, useState } from "react";
import Loading from "../extras/loading";

interface address {
  city: string
  country: string
  name: string
  number: number
  street: string
}

export default function User({ userData }: { userData: Account }) {
  const router = useRouter();
  const [loading, setLoading] = useState(true);
  const [account, setAccount] = useState({});
  const [address, setAddress] = useState<address>();

  useEffect(() => {
    const accountString = localStorage.getItem('account');
    if (accountString !== null) {
      setAccount(JSON.parse(accountString));
    }
  }, []);

  useEffect(() => {
    //@ts-ignore
    if (account && account.state && account.state.account && account.state.account.addressId) {
      //@ts-ignore
      getAddresses(account.state.account.addressId)
        .then(r => setAddress(r))
        .catch(error => console.error("Une erreur s'est produite lors de la récupération des adresses:", error));
      setLoading(false);
    }
  }, [account]);

  if (loading) return <Loading />;

  return (
    <div className="ml-44 mr-44 mt-10">
      <div className="px-4 sm:px-0">
        <h3 className="text-base font-semibold leading-7 text-gray-900">Bienvenue dans votre espace personnel</h3>
        <p className="mt-1 max-w-2xl text-sm leading-6 text-gray-500"></p>
      </div>
      <div className="mt-6 border-t border-gray-100">
        <dl className="divide-y divide-gray-100">
          <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
            <dt className="text-sm font-medium leading-6 text-gray-900">Nom</dt>
            <dd
              className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">{userData.firstName} {userData.lastName}</dd>
          </div>
          <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
            <dt className="text-sm font-medium leading-6 text-gray-900">Email</dt>
            <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">{userData.email}</dd>
          </div>
          <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
            <dt className="text-sm font-medium leading-6 text-gray-900">Numero de telephone</dt>
            <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">{userData.phoneNumber}</dd>
          </div>
          <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
            <dt className="text-sm font-medium leading-6 text-gray-900">Mes adresses</dt>
            <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
              {address == null ? (
                <Button text="Ajouter une adresse" onClick={(() => {
                  router.push('/account/address/new');
                })}/>
              ) : <p> {`${address?.street} ${address?.city} ${address?.country}`} </p>}
            </dd>
          </div>
          <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
            <dt className="text-sm font-medium leading-6 text-gray-900">Mes commandes</dt>
            <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
              <Button text="Gérer mes commandes" onClick={(() => {
                router.push('/account/order');
              })}/>
            </dd>
          </div>
          <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
            <dt className="text-sm font-medium leading-6 text-gray-900"></dt>
            <dd className="mt-1 text-sm leading-6 text-gray-700 sm:col-span-2 sm:mt-0">
              <Button onClick={(async () => {await Logout();})} text="Se déconnecter"/></dd>
          </div>

        </dl>
      </div>
    </div>
  )
}

