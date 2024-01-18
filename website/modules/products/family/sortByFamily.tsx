import { useEffect, useState } from "react";
import { getFamily } from "../../../services/api/products/productService";

interface Family {
  id: number;
  name: string;
}

interface SortByFamilyProps {
  onFamilyChange: (familyId: number) => void;
}

export default function SortByFamily({ onFamilyChange }: SortByFamilyProps){
  const [family, setFamily] = useState<Family[]>([])
  const [loading, setLoading] = useState(true);
  const [isFamilyOpen, setIsFamilyOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const familyData = await getFamily();
        setFamily(familyData);
        setLoading(false);
      } catch (error) {
        console.error('Error during family retrieval:', error);
        setLoading(false);
      }
    };
    fetchData();
  },[]);

  const handleFamilyChange = (familyId: number) => {
    setIsFamilyOpen(false);
    onFamilyChange(familyId);
  };

  return(
    <div>
      <button
        onClick={() => setIsFamilyOpen(!isFamilyOpen)}
        className="text-gray-700 hover:bg-gray-50 border-b border-gray-100 md:hover:bg-transparent md:border-0 pl-3 pr-4 py-2 md:hover:text-blue-700 md:p-0 font-medium flex items-center justify-between w-full md:w-auto">
        Nos vins
        </button>
      <div className={!isFamilyOpen ? `hidden` : `bg-white text-base z-10 list-none divide-y divide-gray-100 rounded shadow my-4 w-44`}>
        <ul>
          {family.map((family) => (
            <li key={family.id}>
              <div onClick={() => handleFamilyChange(family.id)}>
                {family.name}
              </div>
            </li>
          ))}
        </ul>
        <div className="py-1">
          <a href={"/listingProducts"} className="text-sm hover:bg-gray-100 text-gray-700 block px-4 py-2">Tout nos vins</a>
        </div>
      </div>

    </div>
  )

}