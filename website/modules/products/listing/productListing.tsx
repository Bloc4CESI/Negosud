import React, { useEffect, useState } from "react";
import Cart from "../../svg/cart.svg";
import { getFamilyById, getProducts } from "../../../services/api/products/productService";
import SortByFamily from "../family/sortByFamily";
import Loading from "../../extras/loading";
import Link from "next/link";

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

  useEffect(() => {
    const fetchData = async () => {
      try {
        const productsData = await getProducts();
        setProducts(productsData);
        setIsLoading(false);
      } catch (error) {
        console.error('Error during product retrieval:', error);
        setIsLoading(false);
      }
    };
    fetchData();
  }, []);

  const handleFamilyChange = async (familyId: number) => {
    setIsLoading(true);
    try {
      const productsData = await getFamilyById(familyId);
      if (Array.isArray(productsData)) {
        setProducts(productsData);
      } else {
        console.error('Invalid data format received for products:', productsData);
      }
    } catch (error) {
      console.error('Error during product retrieval:', error);
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) return <Loading />;

  return (
    <div>
      <SortByFamily onFamilyChange={handleFamilyChange}/>
      <ul className="flex flex-wrap">
        {products.map((product) => (
          <a key={product.id} href={`/listingProducts/product/${product.id}`}>
        <li className="sm:w-1/2 md:w-1/3 lg:w-1/4 relative m-5 flex w-full max-w-xs flex-col overflow-hidden rounded-lg border border-gray-100 bg-white shadow-md" key={product.id}>
          <img className="object-cover w-full h-full" src={product.image} alt="product image" />
        <div className="mt-4 px-5 pb-5">
          <h6 className="text-xl tracking-tight text-slate-900">{product.home}</h6>
            <h5 className="text-xl tracking-tight text-slate-900">{product.name}</h5>
          <div className="mt-2 mb-5 flex items-center justify-between">
            <p>
              <span className="text-3xl font-bold text-slate-900">{product.price}€</span>
            </p>
          </div>
          <a href="#" className="flex items-center justify-center rounded-md bg-slate-900 px-5 py-2.5 text-center text-sm font-medium text-white hover:bg-gray-700 focus:outline-none focus:ring-4 focus:ring-blue-300">
            <Cart className="mr-2 h-6 w-6"/>
            Add to cart
          </a>
        </div>
        </li>
          </a>
        ))}
      </ul>
    </div>
  );
};

// @ts-ignore
export default ProductListing;