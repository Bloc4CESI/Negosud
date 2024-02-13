"use client"
import { Account } from "../../services/types/types";
import Button from "../extras/button";
import { Logout } from "../../services/api/user/userService";
import { useRouter } from "next/navigation";

export default function User({ userData }: { userData: Account }) {
  const router= useRouter();

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
              {userData.addressId === null ? (
                <Button text="Ajouter une adresse" onClick={(() => {
                  router.push('/account/address');
                })}/>
              ) : <Button text="Gérer mes adresses" onClick={(() => {
                router.push('/account/address');
              })}/>}

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
              <Button onClick={(async () => {
                const r = await Logout();
              })} text="Se déconnecter"/></dd>
          </div>

        </dl>
      </div>
    </div>
  )
}
