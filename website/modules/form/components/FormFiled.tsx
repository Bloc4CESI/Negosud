import { ForwardedRef, forwardRef, HTMLProps } from "react";
import clsxm from "utils/clsxm";

export interface FormFieldPropsInterface extends HTMLProps<HTMLInputElement> {
  label: string;
  error?: string;
  description?: React.ReactNode;
  descriptionUnderInput?: boolean;
  isRequired?: boolean;
}

// eslint-disable-next-line react/display-name
const FormField = forwardRef(
  (
    { label, error, description, descriptionUnderInput, isRequired, ...rest }: FormFieldPropsInterface,
    ref: ForwardedRef<HTMLInputElement>,
  ) => {
    return (
      <div className="border-zinc-600 sm:grid sm:grid-cols-3 sm:items-center sm:gap-4 sm:border-b sm:p-3">
        <label
          htmlFor={rest.id || label}
          className={clsxm(" block text-sm font-medium sm:mt-px", error && "text-red-700")}
        >
          {label}
          {isRequired ? "*" : null}
        </label>
        <div className="mt-1 sm:col-span-2 sm:mt-0">
          <input
            name={rest.id || label}
            id={rest.id || label}
            {...rest}
            ref={ref}
            autoComplete="on"
            className={clsxm(
              "block w-full rounded-md border shadow-sm placeholder:text-zinc-400 focus:border-teal-400 focus:ring-0 focus:ring-teal-400 sm:max-w-md sm:text-sm",
              error && "border-red-700 text-red-700 focus:border-red-500 focus:ring-red-500",
            )}
          />
          <div>{error ? <span className="text-xs text-red-700">{error}</span> : null}</div>
        </div>
        {description && (
          <>
            {descriptionUnderInput ? <div className="sm:col-span-1"></div> : null}
            <div className={descriptionUnderInput ? "sm:col-span-2" : "sm:col-span-3"}>
              <div className="mt-2 text-sm">{description}</div>
            </div>
          </>
        )}
      </div>
    );
  },
);

export default FormField;
