"use client"
import { InputText } from "../form/components/inputText";
import { useForm } from "react-hook-form";
import { createUser } from "../../services/api/user/userService";

type FormValues = {
  LastName: string;
  FirstName: string;
  PhoneNumber: number;
  Email: string;
  Password: string;
};
export default function RegisterForm(){
   const { register, handleSubmit } = useForm<FormValues>({
     defaultValues: {
       FirstName: '',
       LastName: '',
       PhoneNumber: 0,
       Email: '',
       Password: "",
     },
   });

  return(
    <div>
      <h1>Inscription</h1>
      <form onSubmit={handleSubmit(async (formData) => {
       const response = await createUser(formData)
        console.log(response);
        console.log(response);
      })}>
        <div>
          <InputText
            type="texte"
            id="lastName"
            name="lastName"
            placeholder="Entrez votre nom"
            register={register("LastName")}
          />
        </div>
        <div>
          <InputText
            type="text"
            id="firstName"
            name="firstName"
            placeholder="Entrez votre prenom"
            register={register("FirstName")}
          />
        </div>
        <div>
          <InputText
            type="number"
            id="PhoneNumber"
            name="PhoneNumber"
            placeholder="Entrez votre téléphone"
            register={register("PhoneNumber")}
          />
        </div>
        <div>
          <InputText
            type="email"
            id="email"
            name="email"
            placeholder="Entrez votre mail"
            register={register("Email")}
          />
        </div>
        <div>
          <InputText
            type="password"
            id="password"
            name="password"
            placeholder="Entrez votre mot de passe"
            register={register("Password")}
          />
        </div>
        <div>
          <button type="submit" id="submit">Se connecter</button>
        </div>
      </form>
    </div>
  )
};