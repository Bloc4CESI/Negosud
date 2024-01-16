"use client"
import { InputText } from "../form/components/inputText";
import { useForm } from "react-hook-form";
import { createUser } from "../../services/api/user/userService";
import  Arrow  from "../svg/arrowRight.svg"
import React from "react";

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
    <div className="flex flex-col items-center">
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
          <button type="submit" id="submit" className="py-3 px-4 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-gray-200 bg-white text-black shadow-sm hover:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none">
            S&apos;inscrire
           <Arrow/>
          </button>
        </div>
      </form>
    </div>
  )
};