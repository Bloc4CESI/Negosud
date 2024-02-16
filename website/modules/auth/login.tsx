"use client"
import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { getUser } from "../../services/api/user/userService";
import { useRouter } from "next/navigation";
import Arrow from "../svg/arrowRight.svg";
import { useAuth } from "../../services/api/user/useAuth";
import AuthToggle from "./AuthToggle";
import LoadingButton from "../extras/buttonLoading";

const Login = ({ handleToggle, isChecked } : { handleToggle: any, isChecked: any }) => {
  const { authenticate, status } = useAuth();
  const [connected, setConnected] = useState(false);
  const [error, setError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  let errorText;
  const router = useRouter();
  const { register, handleSubmit, formState: { errors }, trigger } = useForm({
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
        setIsLoading(true);
        try {
          const data = await getUser(formData.Email, formData.Password);
          if (data) {
            await authenticate(encodeURIComponent(formData.Email));
            router.push("/account");
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
            className="relative overflow-hidden md:flex w-1/2 bg-[url(https://c1.wallpaperflare.com/preview/835/704/875/drink-fruit-glass-grape.jpg)] i justify-around items-center hidden">
          </div>
          <div className="flex md:w-1/2 justify-center py-10 items-center bg-white">
            <div className="bg-white">
              <h1 className="text-gray-800 font-bold text-2xl mb-7">Encore vous?</h1>
              <div className={`flex items-center border-2 py-2 px-3 rounded-2xl ${errors.Email ? 'border-red-500 mb-1' : 'mb-4'}`}>
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" fill="none"
                     viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2"
                        d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207" />
                </svg>
                <input
                className="pl-2 outline-none"
                type="email"
                placeholder="Email"
                {...register("Email", {
                  required: "Veuillez entrer une adresse email.",
                  pattern: {
                    value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
                    message: "Veuillez entrer une adresse email valide."
                  }
                })}
                onBlur={() => trigger("Email")} // Déclenche la validation onBlur
                required
              />
              </div>
              {errors.Email && <p className="text-red-500 mb-3">{errors.Email.message}</p>}
              <div className={`flex items-center border-2 py-2 px-3 rounded-2xl ${errors.Password ? 'border-red-500 mb-1' : 'mb-4'}`}>
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" viewBox="0 0 20 20"
                     fill="currentColor">
                  <path fillRule="evenodd"
                        d="M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z"
                        clipRule="evenodd" />
                </svg>
                <input
                className="pl-2 outline-none"
                type="text"
                placeholder="Mot de passe"
                {...register("Password", {
                  required: "Veuillez entrer un mot de passe.",
                })}
                onBlur={() => trigger("Password")} // Déclenche la validation onBlur
                required
              />
              </div>
              {errors.Password && <p className="text-red-500 mb-3">{errors.Password.message}</p>}
              {isLoading ? <LoadingButton className="block w-full bg-zinc-800 mt-4 py-2 rounded-2xl text-white font-semibold mb-2"/> : (
              <button type="submit"
                      className="block w-full bg-zinc-800 mt-4 py-2 rounded-2xl text-white font-semibold mb-2">Me connecter
              </button>
              )}
              {error ? <p>{error}</p> : null}
              <AuthToggle className="cursor-pointer" onToggle={handleToggle} isLogin={isChecked} text={"Je n'ai pas de compte"}/>
            </div>
          </div>
        </div>
      </form>
  );
};

export default Login;
