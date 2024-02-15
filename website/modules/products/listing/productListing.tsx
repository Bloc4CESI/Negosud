import React, { useEffect, useState } from "react";
import Cart from "../../svg/cart.svg";
import { getFamilyById, getProducts } from "../../../services/api/products/productService";
import SortByFamily from "../family/sortByFamily";
import Loading from "../../extras/loading";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { TrashIcon } from "@heroicons/react/24/outline";
import { XMarkIcon } from "@heroicons/react/24/solid";
import { fetchData } from "next-auth/client/_utils";
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
  const router = useRouter();

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (selectedFamily) {
          const productsData = await getFamilyById(selectedFamily);
          if (Array.isArray(productsData)) {
            setProducts(productsData);
          } else {
            console.error('Invalid data format received for products:', productsData);
          }
        } else {
        const productsData = await getProducts();
        setProducts(productsData);
        setIsLoading(false);
      }}
      catch (error) {
        console.error('Error during product retrieval:', error);
        setIsLoading(false);
      }
    };
    fetchData();
  }, []);

  const handleFamilyChange = async (e: React.SetStateAction<number | undefined>) => {
    if (e === undefined) {
      const productsData = await getProducts();
      setProducts(productsData);
      setIsLoading(false);
    } else {
      const productsData = await getFamilyById(e);
      setSelectedFamily(e);
      if (Array.isArray(productsData)) {
        setProducts(productsData);
      } else {
        console.error('Invalid data format received for products:', productsData);
      }
    }
  };

  if (isLoading) return <Loading />;
  return (
    <div className="m-16 mt-2">
      <div className="p-5 mb-5 border-b-2 border-b-zinc-800 flex justify-between">
        <div>
         <h2 className="text-5xl flex justify-center">NOS PRODUITS</h2>
        </div>
        <div className="flex items-end">
          <SortByFamily onFamilyChange={handleFamilyChange} />
        </div>
      </div>
      <ul className="flex flex-wrap">
        {products.map((product) => (
          <li onClick={(() => {
            router.push(`listingProducts/product/${product.id}`);
          })}
              className="transition duration-300 ease-in-out hover:scale-105 sm:w-1/2 md:w-1/3 lg:w-1/4 relative m-5 flex w-full cursor-pointer max-w-xs flex-col overflow-hidden rounded-lg border border-gray-100 bg-white shadow-md"
              key={product.id}>
          <img className="object-cover w-full h-full" src={product.image} alt="product image" />
        <div className="mt-4 px-5 pb-5">
            <h5 className="text-xl tracking-tight text-slate-900">{product.name}</h5>
          <div className="mt-2 mb-5 flex items-center justify-between">
            <p>
              <span className="text-3xl font-bold text-slate-900">{product.price}€</span>
            </p>
          </div>
          <a href={`listingProducts/product/${product.id}`} className="flex items-center justify-center rounded-md bg-slate-900 px-5 py-2.5 text-center text-sm font-medium text-white hover:bg-gray-700 focus:outline-none focus:ring-4 focus:ring-blue-300">
            <Cart className="mr-2 h-6 w-6"/>
            Plus d'informations
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