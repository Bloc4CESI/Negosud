"use client"
import React from "react";
import { InputText } from "../form/components/inputText";
import { useForm } from "react-hook-form";
import { getUser } from "../../services/api/user/userService";
import { useRouter } from "next/navigation";
import Arrow from "../svg/arrowRight.svg";

const Login = () => {
  const router = useRouter();
  const { register, handleSubmit } = useForm({
    defaultValues: {
      Email: '',
      Password: "",
    },
  });


  return (
    <div className="flex flex-col items-center">
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
          <button
            type="submit"
            id="submit"
            className="py-3 px-4 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-gray-200 bg-white text-black shadow-sm hover:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none"
          >
            Se connecter
            <Arrow/>
          </button>
        </div>
      </form>
      <div>
      </div>
    </div>
  );
};

export default Login;
