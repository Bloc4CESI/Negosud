import { createAddress } from "../../../services/api/user/userService";
import { useForm } from "react-hook-form";
import { useState } from "react";
import toast from "react-hot-toast";
import { Button } from "@mui/base";
import { useRouter } from "next/navigation";
import { ArrowRightIcon, TruckIcon } from "@heroicons/react/24/outline";
import LoadingButton from "../../extras/buttonLoading";

export default function AddressForm() {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const form = useForm();
  const accountData = localStorage.getItem('account');

  let infoClient:
    { state:
      { account:
          {
            id: number;
            address: string;
            addressId: number;
            email: string;
            firstName: string;
            lastName: string;
            password: string;
            phoneNumber: string;
          };
      };
    }

  if (accountData !== null) {
    infoClient = JSON.parse(accountData);
  } else {
    router.push('/login');
  }

  const onSubmit = form.handleSubmit(
    async () => {
      setIsLoading(true);
      await createAddress({
        name: form.getValues("name"),
        street: form.getValues("street"),
        number: form.getValues("number"),
        city: form.getValues("city"),
        country: form.getValues("country"),
      }, infoClient.state.account)
      setIsLoading(false);
      toast.success("Adresse créée avec succès");
      router.push('/account')
    },
(errors) => {
      toast.success("Problème lors de la creation de l'adresse");
      setIsLoading(false);
    },
  )

  return (
      <form onSubmit={onSubmit} className="bg-white w-full max-w-3xl mb-4 mx-auto px-4 lg:px-6 py-8 shadow-md rounded-md flex flex-col lg:flex-row">
        <div className="w-full lg:pr-8 lg:border-r-2 lg:border-slate-300">
          <div className="mb-8 flex flex-row items-center">
            <h2 className="font-bold text-2xl mr-2">NOUVELLE ADRESSE</h2>
            <TruckIcon className="h-7 w-7"/>
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Nom de l&apos;adresse</label>
            <input
              type="text"
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("name")}
              required
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Rue</label>
            <input
              type="text"
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("street")}
              required
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Code postal</label>
            <input
              type="number"
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("number")}
              required
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Ville</label>
            <input
              type="text"
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("city")}
              required
            />
          </div>
          <div className="mb-4">
            <label className="text-neutral-800 font-bold text-sm mb-2 block">Pays</label>
            <input
              type="text"
              className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
              {...form.register("country")}
              required
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
  );
}