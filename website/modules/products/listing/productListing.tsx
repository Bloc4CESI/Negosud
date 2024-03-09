import React, { useEffect, useState } from "react";
import Cart from "../../svg/cart.svg";
import { getFamilyById, getProductByName, getProducts } from "../../../services/api/products/productService";
import SortByFamily from "../family/sortByFamily";
import Loading from "../../extras/loading";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { TrashIcon } from "@heroicons/react/24/outline";
import { XMarkIcon } from "@heroicons/react/24/solid";
import { fetchData } from "next-auth/client/_utils";
import SearchBar from "../family/searchBar";
import SortByPrice from "../family/sortByPrice";
interface Product {
  id: number;
  name: string;
  price: number;
  image: string;
  description: string;
  dateProduction: string;
  nbProductBox: number;
  home: string;
  familyId: number;
  family: {
    id: number;
    name: string;
    products: Product[];
  };
  providerId: number;
  provider: {
    id: number;
    name: string;
    phoneNumber: string;
    email: string;
    addressId: number;
    address: any;
    products: Product[];
  };
  stock: {
    id: number;
    quantity: number;
    minimum: number;
    maximum: number;
    autoOrder: boolean;
    productId: number;
  };
}

export const ProductListing = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedFamily, setSelectedFamily] = useState<number>();
  const [searchTerm, setSearchTerm] = useState<string>();
  const [sortOrder, setSortOrder] = useState<string>();
  const router = useRouter();

  useEffect(() => {
    const fetchData = async () => {
      try {
        console.log(selectedFamily);
        if (selectedFamily) {
          const productsData = await getFamilyById(selectedFamily,sortOrder);
          if (Array.isArray(productsData)) {
            setProducts(productsData);
          } else {
            console.error('Invalid data format received for products:', productsData);
          }
        } else {
        const productsData = await getProducts(sortOrder);
        setProducts(productsData);
        setIsLoading(false);
      }}
      catch (error) {
        console.error('Error during product retrieval:', error);
        setIsLoading(false);
      }
    };
    fetchData();
  }, [selectedFamily, sortOrder]);
  const handleFamilyChange = async (e: React.SetStateAction<number | undefined>) => {
    if (e === undefined) {
      const productsData = await getProducts(sortOrder);
      setProducts(productsData);
      setIsLoading(false);
    } else {
      const productsData = await getFamilyById(e,sortOrder);
      if (Array.isArray(productsData)) {
        setProducts(productsData);
      } else {
        console.error('Invalid data format received for products:', productsData);
      }
    }
    setSelectedFamily(e);
  };

  const handleReset = async () => {
    setSearchTerm(undefined);
  }

  const handleProductNameChange = async (e: React.SetStateAction<string | undefined>) => {
    setSearchTerm(e);
  }

  if (isLoading) return <Loading />;
  return (
    <div className="m-16 mt-2">
      <div className="p-5 mb-5 border-b-2 border-b-zinc-800 flex justify-between">
        <div>
         <h2 className="text-5xl flex justify-center">NOS PRODUITS</h2>
        </div>
        <div className="flex items-center">
          <SearchBar onSearch={handleProductNameChange}/>
          <SortByFamily onFamilyChange={handleFamilyChange} handleReset={handleReset}/>
          <SortByPrice onSortChange={setSortOrder} /> 
        </div>
      </div>
      <ul className="flex flex-wrap">
        {products.filter(value => {
          return value.name.toLowerCase().includes(searchTerm ? searchTerm.toLowerCase() : "");
        }).map((value) => (
          <li onClick={(() => {
            router.push(`listingProducts/product/${value.id}`);
          })}
              className="transition duration-300 ease-in-out hover:scale-105 sm:w-1/3 md:w-1/4 lg:w-1/5 relative m-5 flex w-full cursor-pointer max-w-xs flex-col overflow-hidden rounded-lg border border-gray-100 bg-white shadow-md"
              key={value.id}>
          <img className="object-cover w-full h-full" src={value.image} alt="product image" />
        <div className="mt-4 px-5 pb-5">
            <h5 className="text-xl tracking-tight text-slate-900">{value.name}</h5>
          <div className="mt-2 mb-5 flex items-center justify-between">
            <p>
              <span className="text-1xl text-slate-900">{new Date(value.dateProduction).toLocaleDateString("fr-FR", {
                year: "numeric",
              })}</span>
            </p>
            <p>
              <span className="text-3xl font-bold text-slate-900">{value.price}€</span>
            </p>
          </div>
          <a href={`listingProducts/product/${value.id}`} className="flex items-center justify-center rounded-md bg-slate-900 px-5 py-2.5 text-center text-sm font-medium text-white hover:bg-gray-700 focus:outline-none focus:ring-4 focus:ring-blue-300">
            <Cart className="mr-2 h-6 w-6"/>
            Plus d&apos;informations
          </a>
        </div>
        </li>
        ))}
      </ul>
    </div>
  );
};
// @ts-ignore
export default ProductListing;