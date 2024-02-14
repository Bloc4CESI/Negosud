import { InputText } from "../form/components/inputText";
import { useForm } from "react-hook-form";
import { createUser } from "../../services/api/user/userService";
import  Arrow  from "../svg/arrowRight.svg"
import React, { useState } from "react";
import { useAuth } from "../../services/api/user/useAuth";
import { useRouter } from "next/navigation";
import { PrismaClient } from "@prisma/client";
import AuthToggle from "./AuthToggle";

type FormValues = {
  LastName: string;
  FirstName: string;
  PhoneNumber: number;
  Email: string;
  Password: string;
};
export default function RegisterForm({ handleToggle, isChecked } : { handleToggle: any, isChecked: any }){
  const prisma = new PrismaClient();
  const [error, setError] = useState(false);
  let errorText = "";
  const {authenticate} = useAuth();
  const router = useRouter();
  const { register, handleSubmit } = useForm<FormValues>();

  return(
      <form className="w-full" onSubmit={handleSubmit(async (formData) => {
        try {
          const data = await createUser(formData)
          if (data) {
            await authenticate(formData.Email);
            router.push('/account');
          } else {
            setError(true);
            errorText = data;
          }
        } catch (error) {
          setError(true);
        }
      })}>
        <div className="h-screen md:flex">
          <div
            className="relative overflow-hidden md:flex w-1/2 bg-gradient-to-tr from-blue-800 to-purple-700 i justify-around items-center hidden">
            <div>
              <h1 className="text-white font-bold text-4xl font-sans">GoFinance</h1>
              <p className="text-white mt-1">The most popular peer to peer lending at SEA</p>
              <button type="submit"
                      className="block w-28 bg-white text-indigo-800 mt-4 py-2 rounded-2xl font-bold mb-2">Read More
              </button>
            </div>
            <div
              className="absolute -bottom-32 -left-40 w-80 h-80 border-4 rounded-full border-opacity-30 border-t-8"></div>
            <div
              className="absolute -bottom-40 -left-20 w-80 h-80 border-4 rounded-full border-opacity-30 border-t-8"></div>
            <div
              className="absolute -top-40 -right-0 w-80 h-80 border-4 rounded-full border-opacity-30 border-t-8"></div>
            <div
              className="absolute -top-20 -right-20 w-80 h-80 border-4 rounded-full border-opacity-30 border-t-8"></div>
          </div>
          <div className="flex md:w-1/2 justify-center py-10 items-center bg-white">
            <div className="bg-white">
              <h1 className="text-gray-800 font-bold text-2xl mb-7">Bienvenue</h1>
              <div className="flex items-center border-2 py-2 px-3 rounded-2xl mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" viewBox="0 0 20 20"
                     fill="currentColor">
                  <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z"
                        clipRule="evenodd" />
                </svg>
                <InputText className="pl-2 outline-none border-none" type="text" placeholder="Nom"
                           register={register("FirstName")} required />
              </div>
              <div className="flex items-center border-2 py-2 px-3 rounded-2xl mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" fill="none"
                     viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2"
                        d="M12 11c0 3.517-1.009 6.799-2.753 9.571m-3.44-2.04l.054-.09A13.916 13.916 0 008 11a4 4 0 118 0c0 1.017-.07 2.019-.203 3m-2.118 6.844A21.88 21.88 0 0015.171 17m3.839 1.132c.645-2.266.99-4.659.99-7.132A8 8 0 008 4.07M3 15.364c.64-1.319 1-2.8 1-4.364 0-1.457.39-2.823 1.07-4" />
                </svg>
                <InputText className="pl-2 outline-none border-none" type="text" placeholder="Prenom"
                           register={register("LastName")} required />
              </div>
              <div className="flex items-center border-2 py-2 px-3 rounded-2xl mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" fill="none"
                     viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2"
                        d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207" />
                </svg>
                <InputText className="pl-2 outline-none border-none" type="text"
                           placeholder="Email"
                           register={register("Email")}
                           required
                />
              </div>
              <div className="flex items-center border-2 py-2 px-3 rounded-2xl mb-4">
                    <svg className="w-4 h-4 text-gray-500 dark:text-gray-400" aria-hidden="true"
                         xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 19 18">
                      <path
                        d="M18 13.446a3.02 3.02 0 0 0-.946-1.985l-1.4-1.4a3.054 3.054 0 0 0-4.218 0l-.7.7a.983.983 0 0 1-1.39 0l-2.1-2.1a.983.983 0 0 1 0-1.389l.7-.7a2.98 2.98 0 0 0 0-4.217l-1.4-1.4a2.824 2.824 0 0 0-4.218 0c-3.619 3.619-3 8.229 1.752 12.979C6.785 16.639 9.45 18 11.912 18a7.175 7.175 0 0 0 5.139-2.325A2.9 2.9 0 0 0 18 13.446Z" />
                    </svg>
                  <input type="text" id="phone-input" aria-describedby="helper-text-explanation"
                         className="pl-2 outline-none border-none"
                         placeholder="06 48 93 40 10"
                         pattern="^0[0-9]{10}"
                         {...register("PhoneNumber")}
                         required
                  />
              </div>
                <div className="flex items-center border-2 py-2 px-3 rounded-2xl">
                  <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" viewBox="0 0 20 20"
                       fill="currentColor">
                    <path fillRule="evenodd"
                          d="M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z"
                          clipRule="evenodd" />
                  </svg>
                  <InputText className="pl-2 outline-none border-none" type="password" placeholder="Mot de passe"
                             register={register("Password")} required />
                </div>
                <button type="submit"
                        className="block w-full bg-indigo-600 mt-4 py-2 rounded-2xl text-white font-semibold mb-2">Créer mon compte
                </button>
                <AuthToggle className="cursor-pointer" onToggle={handleToggle} isLogin={isChecked} text={"J'ai déjà un compte"} />
            </div>
          </div>
        </div>
      </form>
)
};