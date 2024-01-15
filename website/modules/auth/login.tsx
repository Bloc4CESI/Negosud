"use client"
import React from "react";
import { InputText } from "../form/components/inputText";
import { useForm } from "react-hook-form";
import { getUser } from "../../services/api/user/userService";
import { useRouter } from "next/navigation";

const Login = () => {
  const router = useRouter();
  const { register, handleSubmit } = useForm({
    defaultValues: {
      Email: '',
      Password: "",
    },
  });


  return (
    <div>
      <h1>Inscription</h1>
      <form onSubmit={handleSubmit(async (formData) => {
        try {
          const data = await getUser(formData.Email, formData.Password);
          if (data) {
            router.push('/account');
          } else {
            console.log("error");
          }
        } catch (error) {
          console.log(error);
        }
        })}>
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
  );
};

export default Login;
