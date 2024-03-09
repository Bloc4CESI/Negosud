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
  handleReset: () => void;
  error?: string;
}

export default function SortByFamily({ onFamilyChange, handleReset, error, ...rest }: SortByFamilyProps) {
  const [family, setFamily] = useState<Family[]>([]);
  const [selectedFamily, setSelectedFamily] = useState();
  const [selectedNameFamily, setSelectedNameFamily] = useState();


  useEffect(() => {
    const fetchData = async () => {
      try {
        const familyData = await getFamily();
        setFamily(familyData);
      } catch (error) {
        console.log(error);
      }
    };
    fetchData();
  }, []);

  const handleChange = (e: any) => {
    setSelectedFamily(e);
    onFamilyChange(e?e.id:undefined);
    setSelectedNameFamily(e?e.name:undefined);
  }

  return (
    <div className="flex items-center space-x-2">
      <div className="relative w-60">
        <Listbox value={selectedFamily} onChange={handleChange}>
          {({ open }) => (
            <>
              <Listbox.Button className="relative w-full bg-white border border-gray-300 rounded-md shadow-sm pl-3 pr-10 py-2 text-left cursor-default focus:outline-none focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
                <span className="block truncate">{selectedFamily ? selectedNameFamily : "Filtrer par famille"}</span>
                <span className="absolute inset-y-0 right-0 flex items-center pr-2 pointer-events-none">
                  <ChevronUpDownIcon className="h-5 w-5 text-gray-400" aria-hidden="true" />
                </span>
              </Listbox.Button>
              <Transition
                as={Fragment}
                leave="transition ease-in duration-100"
                leaveFrom="opacity-100"
                leaveTo="opacity-0"
                show={open}
              >
<Listbox.Options className="absolute z-10 mt-1 w-full rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm" style={{ maxHeight: "120px", overflowY: "auto" }}>
                  {family.map((family) => (
                    <Listbox.Option
                      key={family.id}
                      value={family}
                      className={({ active }) =>
                        `cursor-pointer select-none relative py-2 pl-10 pr-4 ${
                          active ? 'bg-blue-100 text-blue-900' : 'text-gray-900'
                        }`
                      }
                    >
                      {({ selected, active }) => (
                        <>
                          <span className={`block truncate ${selected ? 'font-medium' : 'font-normal'}`}>
                            {family.name}
                          </span>
                        </>
                      )}
                    </Listbox.Option>
                  ))}
                </Listbox.Options>
              </Transition>
            </>
          )}
        </Listbox>
      </div>
      <XMarkIcon className="h-6 w-6 cursor-pointer hover:scale-110" onClick={() => {
        handleChange(undefined);
      }}/>
    </div>
  );
}