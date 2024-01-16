import React, { InputHTMLAttributes } from "react";
import { UseFormRegisterReturn } from "react-hook-form";

interface Props {
  register: UseFormRegisterReturn;
  error?: string;
}

export const InputText: React.FC<Props & InputHTMLAttributes<HTMLInputElement>> = ({ register, error, ...other }) => {
  return (
    <div>
      <input className="py-3 px-4 block border-gray-200 rounded-lg text-sm focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600" {...other} {...register} />
      <div>{error ? <span className="error">{error}</span> : null}</div>
    </div>
  );
};
