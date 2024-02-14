"use client"
import React, { useEffect, useState } from "react";
import { InputText } from "../form/components/inputText";
import { useForm } from "react-hook-form";
import { getUser } from "../../services/api/user/userService";
import { useRouter } from "next/navigation";
import Arrow from "../svg/arrowRight.svg";
import { useAuth } from "../../services/api/user/useAuth";
import AuthToggle from "./AuthToggle";

const Login = ({ handleToggle, isChecked } : { handleToggle: any, isChecked: any }) => {
  const { authenticate, status } = useAuth();
  const [connected, setConnected] = useState(false);
  const router = useRouter();
  const { register, handleSubmit } = useForm({
    defaultValues: {
      Email: '',
      Password: "",
    },
  });

  useEffect(() => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
    }
  }, []);

  return (
      <form className="w-full" onSubmit={handleSubmit(async (formData) => {
        try {
          const data = await getUser(formData.Email, formData.Password);
          if (data) {
            await authenticate(encodeURIComponent(formData.Email));
            router.push('/account');
          } else {
            console.log("error");
          }
        } catch (error) {
          console.log(error);
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
              <h1 className="text-gray-800 font-bold text-2xl mb-7">Encore vous?</h1>
              <div className="flex items-center border-2 py-2 px-3 rounded-2xl mb-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" fill="none"
                     viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2"
                        d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207" />
                </svg>
                <InputText
                  className="pl-2 outline-none border-none"
                  type="email"
                  placeholder="Email"
                  register={register("Email")}
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
                <InputText
                  className="pl-2 outline-none border-none"
                  type="password"
                  placeholder="Mot de passe"
                  register={register("Password")}
                  required
                />
              </div>
              <button type="submit"
                      className="block w-full bg-indigo-600 mt-4 py-2 rounded-2xl text-white font-semibold mb-2">Me connecter
              </button>
              <AuthToggle className="cursor-pointer" onToggle={handleToggle} isLogin={isChecked} text={"Je n'ai pas de compte"}/>
            </div>
          </div>
        </div>
      </form>
  );
};

export default Login;
