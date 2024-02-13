import FormField from "../form/components/FormFiled";
import { createAddress } from "../../services/api/user/userService";
import { useForm } from "react-hook-form";
import { useState } from "react";
import toast from "react-hot-toast";
import { Button } from "@mui/base";
import Loading from "../extras/loading";
import { useRouter } from "next/navigation";

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
    },
(errors) => {
      toast.success("Problème lors de la creation de l'adresse");
      setIsLoading(false);
    },
  )

  return (
    <>
      <form onSubmit={onSubmit}>
    <FormField label={"Nom de l'adresse"} {...form.register("name")}/>
    <FormField label={"Rue"} {...form.register("street")}/>
    <FormField label={"Code postal"} {...form.register("number")}/>
    <FormField label={"Ville"} {...form.register("city")}/>
    <FormField label={"Pays"} {...form.register("country")}/>
        <Button disabled={isLoading} type="submit">
          {isLoading ? <Loading/> : "Enregistrer"}
        </Button>

      </form>
    </>
  );
}