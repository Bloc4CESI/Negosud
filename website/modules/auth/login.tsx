"use client"
import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { getUser } from "../../services/api/user/userService";
import { useRouter } from "next/navigation";
import Arrow from "../svg/arrowRight.svg";
import { useAuth } from "../../services/api/user/useAuth";
import AuthToggle from "./AuthToggle";
import LoadingButton from "../extras/buttonLoading";
import Modal from "../modal";

const Login = ({ handleToggle, isChecked } : { handleToggle: any, isChecked: any }) => {
  const { authenticate, status } = useAuth();
  const [connected, setConnected] = useState(false);
  const [error, setError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [modalVisible, setModalVisible] = useState(false); 
  const [modalMessage, setModalMessage] = useState(""); 
  const [passwordShown, setPasswordShown] = useState(false);
  let errorText;
  const router = useRouter();
  const { register, handleSubmit, formState: { errors }, trigger} = useForm({
    defaultValues: {
      Email: '',
      Password: "",
    },
  });

  const togglePasswordVisibility = () => {
    setPasswordShown(!passwordShown);
  };

  useEffect(() => {
    if (localStorage.getItem('connected') === null) {
      router.push('/login');
    } else {
      setConnected(true);
    }
  }, []);
  
  const closeModal = () => {
    setModalVisible(false);
    setModalMessage("");
    setIsLoading(false);
  };

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
            setModalMessage("Identifiants non valides. Veuillez réessayer.");
            setModalVisible(true);
                      }
        } catch (error) {
          setError(true);
          setModalMessage("Identifiants non valides. Veuillez réessayer");
          setModalVisible(true); 
          }
      })}>
       {modalVisible && (
          <Modal
          title="Erreur de connexion"
          message={modalMessage}
          type="danger"
          visible={modalVisible}
          onCancel={closeModal}
         onAccept={closeModal}
        />
      )}
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
              <div className={`flex items-center border-2 py-2 px-3 rounded-2xl relative ${errors.Password ? 'border-red-500 mb-1' : 'mb-4'}`}>
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" viewBox="0 0 20 20"
                     fill="currentColor">
                  <path fillRule="evenodd"
                        d="M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z"
                        clipRule="evenodd" />
                </svg>
                <input
                className="pl-2 outline-none"
               type={passwordShown ? 'text' : 'password'}
                placeholder="Mot de passe"
                {...register("Password", {
                  required: "Veuillez entrer un mot de passe.",
                })}
                onBlur={() => trigger("Password")} // Déclenche la validation onBlur
                required
              />
              <div className="absolute inset-y-0 right-0 pr-3 flex items-center">
                <button type="button" onClick={togglePasswordVisibility} className="focus:outline-none">
                    {passwordShown ? (
                      <svg className="h-6 text-gray-700" fill="none" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 512">
                            <path fill="currentColor"
                              d="M572.52 241.4C518.29 135.59 410.93 64 288 64S57.68 135.64 3.48 241.41a32.35 32.35 0 0 0 0 29.19C57.71 376.41 165.07 448 288 448s230.32-71.64 284.52-177.41a32.35 32.35 0 0 0 0-29.19zM288 400a144 144 0 1 1 144-144 143.93 143.93 0 0 1-144 144zm0-240a95.31 95.31 0 0 0-25.31 3.79 47.85 47.85 0 0 1-66.9 66.9A95.78 95.78 0 1 0 288 160z">
                            </path>
                      </svg>
                    ) : (
                      <svg className="h-6 text-gray-700" fill="none" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                      <path fill="currentColor"
                              d="M320 400c-75.85 0-137.25-58.71-142.9-133.11L72.2 185.82c-13.79 17.3-26.48 35.59-36.72 55.59a32.35 32.35 0 0 0 0 29.19C89.71 376.41 197.07 448 320 448c26.91 0 52.87-4 77.89-10.46L346 397.39a144.13 144.13 0 0 1-26 2.61zm313.82 58.1l-110.55-85.44a331.25 331.25 0 0 0 81.25-102.07 32.35 32.35 0 0 0 0-29.19C550.29 135.59 442.93 64 320 64a308.15 308.15 0 0 0-147.32 37.7L45.46 3.37A16 16 0 0 0 23 6.18L3.37 31.45A16 16 0 0 0 6.18 53.9l588.36 454.73a16 16 0 0 0 22.46-2.81l19.64-25.27a16 16 0 0 0-2.82-22.45zm-183.72-142l-39.3-30.38A94.75 94.75 0 0 0 416 256a94.76 94.76 0 0 0-121.31-92.21A47.65 47.65 0 0 1 304 192a46.64 46.64 0 0 1-1.54 10l-73.61-56.89A142.31 142.31 0 0 1 320 112a143.92 143.92 0 0 1 144 144c0 21.63-5.29 41.79-13.9 60.11z">
                            </path>
                      </svg>
                    )}
                </button>
            </div>
              </div>
              {errors.Password && <p className="text-red-500 mb-3">{errors.Password.message}</p>}
              {isLoading ? <LoadingButton className="block w-full bg-zinc-800 mt-4 py-2 rounded-2xl text-white font-semibold mb-2"/> : (
              <button type="submit"
                      className="block w-full bg-zinc-800 mt-4 py-2 rounded-2xl text-white font-semibold mb-2">Me connecter
              </button>
              )}
              {error ? <p>{error}</p> : null}
              <div className="mt-4 flex justify-center"> 
                <AuthToggle className="cursor-pointer text-center underline" onToggle={handleToggle} isLogin={isChecked} text={"Je n'ai pas de compte"} />
              </div>
            </div>
          </div>
        </div>
      </form>
  );
};

export default Login;
