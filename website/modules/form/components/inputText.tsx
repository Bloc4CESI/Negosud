import React, { InputHTMLAttributes } from "react";
import { UseFormRegisterReturn } from "react-hook-form";

interface Props {
  register: UseFormRegisterReturn;
  error?: string;
}

export const InputText: React.FC<Props & InputHTMLAttributes<HTMLInputElement>> = ({ register, error, ...other }) => {
  return (
    <div>
      <input className="input" {...other} {...register} />
      <div>{error ? <span className="error">{error}</span> : null}</div>
    </div>
  );
};
