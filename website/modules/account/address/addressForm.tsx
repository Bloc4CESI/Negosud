import { createAddress } from "../../../services/api/user/userService";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import toast from "react-hot-toast";
import { Button } from "@mui/base";
import { ArrowRightIcon, TruckIcon } from "@heroicons/react/24/outline";
import LoadingButton from "../../extras/buttonLoading";
import { useRouter } from "next/navigation";

export default async function AddressForm() {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const { register, handleSubmit, formState: { errors }, trigger } = useForm<FormValues>();
  const [connected, setConnected] = useState(false);

  useEffect( () => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
      // @ts-ignore
      authenticate(account?.email);
    }
  }, []);

  type FormValues = {
    name: string;
    street: string;
    number: number;
    city: string;
    country: string;
  };

  let infoClient:
    {
      state:
        {
          account:
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


  const onSubmit = handleSubmit(async (formData) => {
    setIsLoading(true);
    try {
      await createAddress(formData, infoClient.state.account);
      toast.success("Adresse enregistrée avec succès !");
      setIsLoading(false);
      router.push('/account');
    } catch (error) {
      setIsLoading(false);
      toast.error("Une erreur s'est produite lors de la création de l'adresse.");
    }
  });

  return (
    <form onSubmit={onSubmit}
          className="bg-white w-full max-w-3xl mb-4 mx-auto px-4 lg:px-6 py-8 shadow-md rounded-md flex flex-col lg:flex-row">
      <div className="w-full lg:pr-8 lg:border-r-2 lg:border-slate-300">
        <div className="mb-8 flex flex-row items-center">
          <h2 className="font-bold text-2xl mr-2">NOUVELLE ADRESSE</h2>
          <TruckIcon className="h-7 w-7" />
        </div>
        <div className={`${errors.name ? 'border-red-500 mb-1' : 'mb-4'}`}>
          <label className="text-neutral-800 font-bold text-sm mb-2 block">Nom de l&apos;adresse</label>
          <input
            type="text"
            className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
            {...register("name", {
              required: "Veuillez entrer un nom.",
              pattern: {
                value: /^[a-zA-Z0-9\s]+$/,
                message: "Veuillez entrer un nom valide."
              }
            })}
            onBlur={() => trigger("name")} // Déclenche la validation onBlur
            required
          />
        </div>
        {errors.name && <p className="text-red-500 mb-3">{errors.name.message}</p>}
        <div className={`${errors.street ? 'border-red-500 mb-1' : 'mb-4'}`}>
          <label className="text-neutral-800 font-bold text-sm mb-2 block">Rue</label>
          <input
            type="text"
            className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
            {...register("street", {
              required: "Veuillez entrer un nom de rue.",
              pattern: {
                value: /^[a-zA-Z0-9\s']+$/,
                message: "Veuillez entrer un nom de rue valide."
              }
            })}
            onBlur={() => trigger("street")} // Déclenche la validation onBlur
            required
          />
        </div>
        {errors.street && <p className="text-red-500 mb-3">{errors.street.message}</p>}
        <div className={`${errors.number ? 'border-red-500 mb-1' : 'mb-4'}`}>
          <label className="text-neutral-800 font-bold text-sm mb-2 block">Code postal</label>
          <input
            type="text"
            className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
            {...register("number", {
              required: "Veuillez entrer un code postal.",
              pattern: {
                value: /^\d{5}$/,
                message: "Veuillez entrer un code postal valide."
              }
            })}
            onBlur={() => trigger("number")} // Déclenche la validation onBlur
            required
          />
        </div>
        {errors.number && <p className="text-red-500 mb-3">{errors.number.message}</p>}
        <div className={`${errors.city ? 'border-red-500 mb-1' : 'mb-4'}`}>
          <label className="text-neutral-800 font-bold text-sm mb-2 block">Ville</label>
          <input
            type="text"
            className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
            {...register("city", {
              required: "Veuillez entrer une ville.",
              pattern: {
                value: /^[a-zA-ZÀ-ÿ\s-]+$/,
                message: "Veuillez entrer une ville valide."
              }
            })}
            onBlur={() => trigger("city")} // Déclenche la validation onBlur
            required
          />
        </div>
        {errors.city && <p className="text-red-500 mb-3">{errors.city.message}</p>}
        <div className={`${errors.country ? 'border-red-500 mb-1' : 'mb-4'}`}>
          <label className="text-neutral-800 font-bold text-sm mb-2 block">Pays</label>
          <input
            type="text"
            className="flex h-10 w-full rounded-md border-2 bg-background px-4 py-1.5 text-lg ring-offset-background placeholder-text-muted-foreground focus-visible:outline-none focus-visible:border-purple-600 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 undefined"
            {...register("country", {
              required: "Veuillez entrer un pays.",
              pattern: {
                value: /^[a-zA-ZÀ-ÿ\s-]+$/,
                message: "Veuillez entrer un pays valide."
              }
            })}
            onBlur={() => trigger("country")} // Déclenche la validation onBlur
            required
          />
        </div>
        {errors.country && <p className="text-red-500 mb-3">{errors.country.message}</p>}
        <div className="mt-10 mb-4">
          {isLoading ? <LoadingButton className="border-2 border-zinc-800 rounded-2xl p-3 flex flex-row items-center" />
            : (
              <Button type="submit" className="border-2 border-zinc-800 rounded-2xl p-3 flex flex-row items-center">
                <p> Enregistrer cette adresse</p>
                <ArrowRightIcon className="h-4 w-4 ml-2" />
              </Button>
            )}
        </div>
      </div>
    </form>
  );
}