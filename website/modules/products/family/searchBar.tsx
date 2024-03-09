import { FaMagnifyingGlass } from "react-icons/fa6";
import { Toaster } from "react-hot-toast";

export default function SearchBar({ onSearch }: { onSearch: (search: string) => void; }){
  return (
    <div className="p-2 relative flex items-center flex-row rounded-md mr-3 w-full md:w-64">
    <FaMagnifyingGlass className="h-6 w-6 p-1" />
    <input
      type="text"
      placeholder="Rechercher"
      className="w-full p-1 ml-2 border-2 border-gray-300 rounded-md focus:border-blue-500 focus:outline-none"
      onChange={(e) => onSearch(e.target.value)}
    />
  </div>
  )
}