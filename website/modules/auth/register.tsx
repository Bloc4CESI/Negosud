import { useForm } from "react-hook-form";
import { createUser } from "../../services/api/user/userService";
import React, { useState } from "react";
import { useAuth } from "../../services/api/user/useAuth";
import { useRouter } from "next/navigation";
import { PrismaClient } from "@prisma/client";
import AuthToggle from "./AuthToggle";
import LoadingButton from "../extras/buttonLoading";
import Modal from "../modal";

type FormValues = {
  LastName: string;
  FirstName: string;
  PhoneNumber: number;
  Email: string;
  Password: string;
};

export default function RegisterForm({ handleToggle, isChecked }: { handleToggle: any, isChecked: any }) {
  const prisma = new PrismaClient();
  const [error, setError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  let errorText = "";
  const { authenticate } = useAuth();
  const router = useRouter();
  const [modalVisible, setModalVisible] = useState(false); 
  const [modalMessage, setModalMessage] = useState(""); 
  const [passwordShown, setPasswordShown] = useState(false);
  const { register, handleSubmit, formState: { errors }, trigger, reset } = useForm<FormValues>();

  const closeModal = () => {
    setModalVisible(false);
    setModalMessage("");
    setIsLoading(false);
  };

  const togglePasswordVisibility = () => {
    setPasswordShown(!passwordShown);
  };

  const onSubmit = async (formData: FormValues) => {
    setIsLoading(true);
    try {
      const res  = await createUser(formData)
      if (res === 200) {
        await authenticate(formData.Email);
        router.push('/account');
      } else {
        setError(true);
        setIsLoading(false);
        setModalVisible(true);
        setModalMessage("L'adresse fournie n'est pas valide ou est déjà associée à un autre client.");        
      }
    } catch (error) {
      setError(true);
      setIsLoading(false);
      setModalVisible(true);
      setModalMessage("L'adresse fournie n'est pas valide ou est déjà associée à un autre client.");      
    }
  };

  return (
    
    <form className="w-full" onSubmit={handleSubmit(onSubmit)}>
      <div className="h-screen md:flex">
        <div className="relative overflow-hidden md:flex rounded w-1/2 bg-[url(https://c1.wallpaperflare.com/preview/835/704/875/drink-fruit-glass-grape.jpg)] i justify-around items-center hidden">
        </div>
        {modalVisible && (
          <Modal
          title="Erreur"
          message={modalMessage}
          type="danger"
          visible={modalVisible}
          onCancel={closeModal}
         onAccept={closeModal}
        />
      )}
        <div className="flex md:w-1/2 justify-center py-10 items-center bg-white">
          <div className="bg-white">
            <h1 className="text-gray-800 font-bold text-2xl mb-7">Bienvenue chez NegoSud</h1>
            <div className={`flex items-center border-2 py-2 px-3 rounded-2xl ${errors.LastName ? 'border-red-500 mb-1' : 'mb-4'}`}>
              <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
              </svg>
              <input
                className="pl-2 outline-none"
                type="text"
                placeholder="Nom"
                {...register("LastName", {
                  required: "Veuillez entrer un nom.",
                  pattern: {
                    value: /^[a-zA-Z-' ]+$/,
                    message: "Veuillez entrer un nom valide."
                  }
                })}
                onBlur={() => trigger("LastName")} // Déclenche la validation onBlur
                required
              />
            </div>
            {errors.LastName && <p className="text-red-500 mb-3">{errors.LastName.message}</p>}
            <div className={`flex items-center border-2 py-2 px-3 rounded-2xl ${errors.FirstName ? 'border-red-500 mb-1' : 'mb-4'}`}>
              <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 11c0 3.517-1.009 6.799-2.753 9.571m-3.44-2.04l.054-.09A13.916 13.916 0 008 11a4 4 0 118 0c0 1.017-.07 2.019-.203 3m-2.118 6.844A21.88 21.88 0 0015.171 17m3.839 1.132c.645-2.266.99-4.659.99-7.132A8 8 0 008 4.07M3 15.364c.64-1.319 1-2.8 1-4.364 0-1.457.39-2.823 1.07-4" />
              </svg>
              <input
                className="pl-2 outline-none"
                type="text"
                placeholder="Prénom"
                {...register("FirstName", {
                  required: "Veuillez entrer un prénom.",
                  pattern: {
                    value: /^[a-zA-Z-' ]+$/,
                    message: "Veuillez entrer un prénom valide."
                  }
                })}
                onBlur={() => trigger("FirstName")} // Déclenche la validation onBlur
                required
              />
            </div>
            {errors.FirstName && <p className="text-red-500 mb-3">{errors.FirstName.message}</p>}
            <div className={`flex items-center border-2 py-2 px-3 rounded-2xl ${errors.Email ? 'border-red-500 mb-1' : 'mb-4'}`}>
              <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207" />
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
            <div className={`flex items-center border-2 py-2 px-3 rounded-2xl ${errors.PhoneNumber ? 'border-red-500 mb-1' : 'mb-4'}`}>
              <svg className="w-4 h-4 text-gray-500 dark:text-gray-400" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 19 18">
                <path d="M18 13.446a3.02 3.02 0 0 0-.946-1.985l-1.4-1.4a3.054 3.054 0 0 0-4.218 0l-.7.7a.983.983 0 0 1-1.39 0l-2.1-2.1a.983.983 0 0 1 0-1.389l.7-.7a2.98 2.98 0 0 0 0-4.217l-1.4-1.4a2.824 2.824 0 0 0-4.218 0c-3.619 3.619-3 8.229 1.752 12.979C6.785 16.639 9.45 18 11.912 18a7.175 7.175 0 0 0 5.139-2.325A2.9 2.9 0 0 0 18 13.446Z" />
              </svg>
              <input
                className="pl-2 outline-none"
                type="text"
                placeholder="Numéro de téléphone"
                {...register("PhoneNumber", {
                  required: "Veuillez entrer un numéro de téléphone.",
                  pattern: {
                    value: /^\d{10}$/,
                    message: "Veuillez entrer un numéro de téléphone valide."
                  }
                })}
                onBlur={() => trigger("PhoneNumber")} // Déclenche la validation onBlur
                required
              />
              </div>
            {errors.PhoneNumber && <p className="text-red-500 mb-3">{errors.PhoneNumber.message}</p>}
            <div className={`flex items-center border-2 py-2 px-3 rounded-2xl  relative ${errors.Password ? 'border-red-500 mb-1' : 'mb-4'}`}>              <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M5 9V7a5 5 0 0110 0v2a2 2 0 012 2v5a2 2 0 01-2 2H5a2 2 0 01-2-2v-5a2 2 0 012-2zm8-2v2H7V7a3 3 0 016 0z" clipRule="evenodd" />
              </svg>
              <input
                className="pl-2 outline-none"
                type={passwordShown ? 'text' : 'password'}
                placeholder="Mot de passe"
                {...register("Password", {
                  required: "Veuillez entrer un mot de passe.",
                  pattern: {
                    value: /.{8,}/,
                    message: "Mot de passe trop court."
                  }
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
            {isLoading ? <LoadingButton className="block w-full bg-zinc-800 mt-4 py-2 rounded-2xl text-white font-semibold mb-2" /> : (
              <button type="submit" className="block w-full bg-zinc-800 mt-4 py-2 rounded-2xl text-white font-semibold mb-2">Créer mon compte</button>
            )}
            {error ? <p>{error}</p> : null}
            <div className="mt-4 flex justify-center"> 
                <AuthToggle className="cursor-pointer underline" onToggle={handleToggle} isLogin={isChecked} text={"J'ai déjà un compte"} />
            </div>  
          </div>
        </div>
      </div>
    </form>
  );
}
