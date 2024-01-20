"use client"
import Header from "../../../../../modules/layout/header";
import Footer from "../../../../../modules/layout/footer";
import '../../../globals.css';
import { useEffect, useState } from "react";
import { getProductByID } from "../../../../../services/api/products/productService";
import { ProductType } from "../../../../../services/types/types";

export default function ProductPage({ params }: { params: { id: number } }) {
    const [product, setProduct] = useState<ProductType>();
console.log(product);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const productsData = await getProductByID(params.id);
                setProduct(productsData);
            } catch (error) {
                console.error('Error during product retrieval:', error);
            }
        };
        fetchData();
    }, []);

    return (
        <div>
            <Header/>
            <div className="bg-white py-8">
                <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex flex-col md:flex-row -mx-4">
                        <div className="md:flex-1 px-4">
                            <div className="h-[460px] rounded-lg bg-gray-300 mb-4">
                                <img src={product?.image} alt="Product" className="w-full h-full object-cover"/>
                            </div>
                            <div className="flex -mx-2 mb-4">
                                <div className="w-1/2 px-2">
                                    <button className="w-full bg-gray-900 text-white py-2 px-4 rounded-full font-bold hover:bg-gray-800">Add to Cart</button>
                                </div>
                                <div className="w-1/2 px-2">
                                    <button className="w-full bg-gray-200 text-gray-800 px-4 rounded-full font-bold hover:bg-gray-300">Add to Wishlist</button>
                                </div>
                            </div>
                            </div>
                        <div className="md:flex-1 px-4">
                            <h2 className="text-2xl font-bold text-black mb-2">{product?.name}</h2>
                            <p className="text-black text-sm mb-4">
                                {product?.home}
                            </p>
                            <div className="flex mb-4">
                                <div className="mr-4">
                                    <span className="font-bold text-black">Price:</span>
                                    <span className="text-black">{product?.price}</span>
                                </div>
                            </div>
                            <div className="mb-4">
                                <span className="font-bold text-black">Select Size:</span>
                                <div className="flex items-center mt-2">
                                    <button className="bg-gray-300 text-gray-700 py-2 px-4 rounded-full font-bold mr-2 hover:bg-gray-400">À l&apos;unité</button>
                                    <button className="bg-gray-300 text-gray-700 py-2 px-4 rounded-full font-bold mr-2 hover:bg-gray-400"> Pack de {product?.nbProductBox}</button>
                                </div>
                            </div>
                            <div>
                                <span className="font-bold text-black">Product Description:</span>
                                <p className="text-black text-sm mt-2">
                                    {product?.description}
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <Footer/>
        </div>
    );
};