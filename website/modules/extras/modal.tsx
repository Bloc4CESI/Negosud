import { Dialog, Transition } from "@headlessui/react";
import { ExclamationTriangleIcon, QuestionMarkCircleIcon } from "@heroicons/react/24/outline";
import { Fragment, ReactNode, useEffect, useState } from "react";
import Button from "../extras/button";

export interface ModalPropsInterface {
  title: string;
  message: string | ReactNode;
  type: "question" | "warning" | "danger";
  visible: boolean;
  onAccept: () => void;
  onCancel?: () => void;
}

export default function Modal({
title,
message,
type = "danger",
visible = false,
onAccept,
onCancel,
}: ModalPropsInterface) {
  const [open, setOpen] = useState(visible);

  useEffect(() => {
    setOpen(visible);
  }, [visible]);

  let iconBackgroundColor;
  let buttonBackgroundColor;

  switch (type) {
    case "question":
      iconBackgroundColor = "bg-teal-400";
      buttonBackgroundColor = "bg-teal-400 hover:bg-teal-500";
      break;
    case "warning":
      iconBackgroundColor = "bg-yellow-400";
      buttonBackgroundColor = "bg-yellow-400 hover:bg-yellow-500";
      break;
    default:
      iconBackgroundColor = "bg-red-400";
      buttonBackgroundColor = "bg-red-400 hover:bg-red-500";
  }

  return (
    <Transition.Root show={open} as={Fragment}>
      <Dialog
        as="div"
        className="relative z-10"
        onClose={(v) => {
          setOpen(v);
          onCancel();
        }}
      >
        <Transition.Child
          as={Fragment}
          enter="ease-out duration-300"
          enterFrom="opacity-0"
          enterTo="opacity-100"
          leave="ease-in duration-200"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <div className="z-5 fixed inset-0 bg-zinc-500 bg-opacity-75 transition-opacity" />
        </Transition.Child>

        <div className="fixed inset-0 z-10 overflow-y-auto">
          <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
            <Transition.Child
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
              enterTo="opacity-100 translate-y-0 sm:scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 translate-y-0 sm:scale-100"
              leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
            >
              <Dialog.Panel className="relative transform overflow-hidden rounded-md bg-zinc-900 text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg">
                <div className="bg-zinc-900 px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                  <div className="sm:flex sm:items-start">
                    <div
                      className={`mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full sm:mx-0 sm:h-10 sm:w-10 ${iconBackgroundColor}`}
                    >
                      {(type === "danger" || type === "warning") && (
                        <ExclamationTriangleIcon className="h-6 w-6 stroke-white" aria-hidden="true" />
                      )}
                      {type === "question" && (
                        <QuestionMarkCircleIcon className="h-6 w-6 stroke-white" aria-hidden="true" />
                      )}
                    </div>
                    <div className="mt-3 w-full text-center sm:ml-4 sm:mt-0 sm:text-left">
                      <Dialog.Title as="h3" className="text-lg font-medium leading-6">
                        {title}
                      </Dialog.Title>
                      <div className="mt-2 w-full">{message}</div>
                    </div>
                  </div>
                </div>
                <div className="flex justify-end gap-2 p-4">
                  <Button variant="secondary" type="button" onClick={onCancel}>
                    Annuler
                  </Button>
                  <Button type="button" className={clsxm(buttonBackgroundColor)} onClick={onAccept}>
                    Continuer
                  </Button>
                </div>
              </Dialog.Panel>
            </Transition.Child>
          </div>
        </div>
      </Dialog>
    </Transition.Root>
  );
}
