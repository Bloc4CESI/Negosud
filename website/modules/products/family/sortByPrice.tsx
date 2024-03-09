import React, { useState } from 'react';
import { Listbox } from '@headlessui/react';
import { ChevronUpDownIcon } from '@heroicons/react/24/solid';

const sortOptions = [
  { id: 0, name: 'Trier par', value: '' },
  { id: 1, name: 'Prix croissant', value: 'price_asc' },
  { id: 2, name: 'Prix décroissant', value: 'price_desc' },
];

interface SortByPriceProps {
  onSortChange: (value: string) => void;
}

export default function SortByPrice({ onSortChange }: SortByPriceProps) {
  // Utilisez le premier élément de sortOptions comme valeur initiale
  const [selectedValue, setSelectedValue] = useState(sortOptions[0].value);

  const handleChange = (value: string) => {
    setSelectedValue(value); // Mettez à jour l'état avec la nouvelle valeur sélectionnée
    onSortChange(value); // Propagez la modification au composant parent
  };

  // Trouvez l'option sélectionnée basée sur la valeur actuelle
  const selectedOption = sortOptions.find(option => option.value === selectedValue);

  return (
    <div className="relative w-60">
      <Listbox value={selectedValue} onChange={handleChange}>
        {({ open }) => (
          <>
            <Listbox.Button className="relative w-full bg-white border border-gray-300 rounded-md shadow-sm pl-3 pr-10 py-2 text-left cursor-default focus:outline-none focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
              <span className="block truncate">{selectedOption?.name}</span>
              <span className="absolute inset-y-0 right-0 flex items-center pr-2 pointer-events-none">
                <ChevronUpDownIcon className="h-5 w-5 text-gray-400" aria-hidden="true" />
              </span>
            </Listbox.Button>
            <Listbox.Options className="absolute z-10 mt-1 max-h-60 w-full overflow-auto rounded-md bg-white py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm">
              {sortOptions.map((option) => (
                <Listbox.Option
                  key={option.id}
                  value={option.value}
                  className={({ active }) =>
                    `cursor-pointer select-none relative py-2 pl-10 pr-4 ${
                      active ? 'bg-blue-100 text-blue-900' : 'text-gray-900'
                    }`
                  }
                >
                  {({ selected }) => (
                    <>
                      <span className={`block truncate ${selected ? 'font-medium' : 'font-normal'}`}>
                        {option.name}
                      </span>
                    </>
                  )}
                </Listbox.Option>
              ))}
            </Listbox.Options>
          </>
        )}
      </Listbox>
    </div>
  );
}
