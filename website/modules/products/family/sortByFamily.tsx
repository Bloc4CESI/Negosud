import React, { Fragment, useEffect, useState } from "react";
import { getFamily } from "../../../services/api/products/productService";
import { Listbox, Transition } from "@headlessui/react";
import { ChevronUpDownIcon, XMarkIcon } from "@heroicons/react/24/solid";

interface Family {
  id: number;
  name: string;
}

interface SortByFamilyProps {
  onFamilyChange: (familyId: number | undefined) => void;
  error?: string;
}

export default function SortByFamily({ onFamilyChange, error, ...rest }: SortByFamilyProps) {
  const [family, setFamily] = useState<Family[]>([]);
  const [selectedFamily, setSelectedFamily] = useState();
  const [selectedNameFamily, setSelectedNameFamily] = useState();
  const [loading, setLoading] = useState(true);


  useEffect(() => {
    const fetchData = async () => {
      try {
        const familyData = await getFamily();
        setFamily(familyData);
        setLoading(false);
      } catch (error) {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  const handleChange = (e: any) => {
    setSelectedFamily(e);
    onFamilyChange(e.id);
    setSelectedNameFamily(e.name);
  }

  return(
    <>
    <Listbox value={selectedFamily} onChange={handleChange}>
      {({ open }) => (
        <div className="relative w-32">
          <Listbox.Button
            className={""}
            {...rest}
          >
              <span className="p-1">{selectedFamily ? selectedNameFamily : "Filtres"}</span>
            <span className="absolute inset-y-0 right-0 flex items-center pr-2">
                <ChevronUpDownIcon className="h-5 w-5" aria-hidden="true" />
              </span>
          </Listbox.Button>

          <Transition
            show={open}
            as={Fragment}
            leave="transition ease-in duration-100"
            leaveFrom="opacity-100"
            leaveTo="opacity-0"
          >
            <Listbox.Options className="absolute border-2 border-zinc-800 z-10 mt-1 max-h-60 w-full overflow-auto rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 sm:text-sm">
              {family.map((family) => (
                <Listbox.Option
                  key={family.id}
                  value={family}
                >
                <span className="p-1 cursor-pointer">
                  {family.name}
                </span>
                </Listbox.Option>
              ))}
            </Listbox.Options>
          </Transition>
        </div>
      )}
    </Listbox>
      <XMarkIcon className="h-6 w-6 cursor-pointer" onClick={(() => {
        setSelectedFamily(undefined);
        setSelectedNameFamily(undefined);
        onFamilyChange(undefined);
      })}/>
    </>
  )
}