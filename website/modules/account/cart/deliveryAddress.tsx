import { createAddress, modifyAddressClient } from "services/api/user/userService";
import { getAddresses } from "services/api/user/userService";
import { useForm } from "react-hook-form";
import { useState } from "react";
import toast from "react-hot-toast";
import { Button } from "@mui/base";
import { useRouter } from "next/navigation";
import { ArrowRightIcon, TruckIcon } from "@heroicons/react/24/outline";
import LoadingButton from "modules/extras/buttonLoading";
import { useEffect } from "react";
import { useAccount } from "services/api/user/useAccount";
import { AddressType, OrderLineType } from "services/types/types";
import CreditCardForm from "modules/extras/creditCard";
import { getOrderClient, putOrderClient } from "services/api/products/cart";

export default function AddressForm() {
    const {account} = useAccount();
  const router = useRouter();
  const [ordersLine, setOrdersLine] = useState<OrderLineType[]>([])
  const [clientAddress, setClientAddress] = useState<AddressType>();
  const [isLoading, setIsLoading] = useState(false);
  const [isPayment, setIsPayment] = useState(false);
  const [isPaymentLoading, setIsPaymentLoading] = useState(false);

  const form = useForm();

    useEffect(() => {
        if (account?.addressId){
            const fetchClientAddress = async () => {
            try {
                const address = await getAddresses(account?.addressId);
                setClientAddress(address);
            } catch (error) {
                console.error('Erreur lors de la récupération de l\'adresse du client:');
            } finally {
                setIsLoading(false);
            }
            };
        
            fetchClientAddress();
    }}, []);

  const onSubmit = form.handleSubmit(
    async () => {
        let data;
      setIsLoading(true);
      if(clientAddress){
        data = {
        id: clientAddress.id,
        name: form.getValues("name") == "" ? clientAddress.name : form.getValues("name"),
        street: form.getValues("street") == "" ? clientAddress.street : form.getValues("street"),
        number: form.getValues("number") == "" ? clientAddress.number : form.getValues("number"),
        city: form.getValues("city") == "" ? clientAddress.city : form.getValues("city"),
        country: form.getValues("country") == "" ? clientAddress.country : form.getValues("country"),
        }
        await modifyAddressClient(data, account?.addressId);
      }else {
        data = {
          name: form.getValues("name"),
          street: form.getValues("street"),
          number: form.getValues("number"),
          city: form.getValues("city"),
          country: form.getValues("country"),  
        }
      await createAddress(data, account);
      }
      const ordersDate = await getOrderClient(account?.id);
      setOrdersLine(ordersDate);
      setIsLoading(false);
      setIsPayment(true);
      toast.success("Adresse créée avec succès");
    },
(errors) => {
      toast.success("Problème lors de la creation de l'adresse");
      setIsLoading(false);
    },
  )

  const handlePaymentClick = async () => {
    setIsPaymentLoading(true);
          setIsLoading(false);
        const total = ordersLine?.reduce((acc, orderline) => acc + orderline.price, 0);
        const data = {
          id: ordersLine[0]?.clientOrder?.id,
          date: new Date(),
          price: total,
          orderStatus: "VALIDE"
        } 
        console.log(data)
        console.log(ordersLine);
        await putOrderClient(data);
        router.push(`/account/order/${ordersLine[0].clientOrder.id}`)
    }

  return (
    <>
    {isPayment ? (
        <CreditCardForm handlePaymentClick={handlePaymentClick}/>
    ) : (
      <form onSubmit={onSubmit} className="bg-white w-full max-w-3xl mb-4 mx-auto px-4 lg:px-6 py-8 shadow-md rounded-md flex flex-col lg:flex-row">
        <div className="w-full lg:pr-8 lg:border-r-2 lg:border-slate-300">
          <div className="mb-8 flex flex-row items-center">
            <h2 className="font-bold text-2xl mr-2">VOTRE ADRESSE</h2>
            <TruckIcon className="h-7 w-7"/>
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Nom de l&apos;adresse</label>
            <input
              type="text"
              placeholder={clientAddress?.name}
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("name")}
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Rue</label>
            <input
              type="text"
              placeholder={clientAddress?.street}
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("street")}
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Code postal</label>
            <input
              type="number"
              placeholder={clientAddress?.number.toString()}
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("number")}
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Ville</label>
            <input
              type="text"
              placeholder={clientAddress?.city}
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("city")}
              
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Pays</label>
            <input
              type="text"
              placeholder={clientAddress?.country}
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("country")}
        
            />
          </div>
          <div className="mt-10 mb-4">
            {isLoading ? <LoadingButton className="border-2 border-zinc-800 rounded-2xl p-3 flex flex-row items-center"/>
            : (
            <Button type="submit" className="border-2 border-zinc-800 rounded-2xl p-3 flex flex-row items-center">
              <p> Enregistrer cette adresse</p>
              <ArrowRightIcon className="h-4 w-4 ml-2"/>
            </Button>
              )}
          </div>
        </div>
      </form>
    )}
    </>
  );
}