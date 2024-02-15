import React, { useEffect, useState } from "react";
import { getProductByID } from "../../../services/api/products/productService";
import { ProductType } from "../../../services/types/types";
import { useAccount } from "../../../services/api/user/useAccount";
import { postOrderClientLine } from "../../../services/api/products/cart";
import { useRouter } from "next/navigation";

export const FormProduct = ({ id }: { id: number }) => {
  const router = useRouter();
    const [product, setProduct] = useState<ProductType | null>(null);
    const { account} = useAccount();
    const [selectedOption, setSelectedOption] = useState<string>("unitaire"); // État pour stocker la valeur sélectionnée
    const [finalPrice, setFinalPrice] = useState<number | null>(null); // État pour stocker le prix final
    const [quantity, setQuantity] = useState<number>(1); // État pour stocker le prix final

  useEffect(() => {
      const fetchData = async () => {
          try {
              const productData = await getProductByID(id);
              setProduct(productData);
              const initialPrice = productData?.price || null;
              setFinalPrice(initialPrice); // Met à jour le prix final
      } catch (error) {
        console.error('Error during product retrieval:', error);
      }
    };

    fetchData();
  }, [id]);

  console.log(account);
  const handleAddToCart = () => {
    if (account == null) {
      router.push('/login')
    } else {
        postOrderClientLine(product?.id,account?.id,quantity,finalPrice)
    }
  }

  const handleOptionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value; // Récupère la valeur de l'option sélectionnée
    setSelectedOption(value); // Met à jour la valeur sélectionnée

    let newPrice = product?.price;
    let newQuantity = 1;// Prix initial (unitaire)

    if (value !== "unitaire" && product != null) {
        // Prix pour le pack avec une réduction de 10%
        newPrice = product?.price * product?.nbProductBox * 0.9;
        newQuantity = product?.nbProductBox;
    }
    if(newPrice && newQuantity){
        setFinalPrice(newPrice); // Met à jour le prix final
        setQuantity(newQuantity);
    }
}

  const dateFr = product?.dateProduction ? new Date(product.dateProduction).toLocaleDateString('fr-FR', {
    year: "numeric",
    month: "long",
  }) : '';

return (
    <div>
        <div className="bg-white py-8">
            <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
                <div className="flex flex-col md:flex-row -mx-4">
                    <div className="md:flex-1 px-4">
                        <div className="h-[460px] rounded-lg bg-gray-300 mb-4">
                            <img src={product?.image} alt="Product" className="w-full h-full object-cover"/>
                        </div>
                        <div className="flex -mx-2 mb-4">
                            <div className="w-full px-2">
                                <button onClick={handleAddToCart} className="w-full bg-gray-900 text-white py-2 px-4 rounded-full font-bold hover:bg-gray-800">Add to Cart</button>
                            </div>
                        </div>
                        </div>
                    <div className="md:flex-1 px-4">
                        <h2 className="text-2xl font-bold text-black mb-2">{product?.name}</h2>
                        <div className="flex mb-4">
                            <div className="mr-4">
                                <span className="font-bold text-black">Famille de vin: </span>
                                <span className="text-black">{product?.family.name}</span>
                            </div>
                        </div>
                        <div className="flex mb-4">
                            <div className="mr-4">
                                <span className="font-bold text-black">Date de production: </span>
                                <span className="text-black">{dateFr}</span>
                            </div>
                        </div>
                        <div className="flex mb-4">
                            <div className="mr-4">
                                <span className="font-bold text-black">Maison: </span>
                                <span className="text-black">{product?.home ? product.home : "Inconnu"}</span>
                            </div>
                        </div>
                        <div className="flex mb-4">
                            <div className="mr-4">
                                <span className="font-bold text-black">Price: </span>
                                <span className="text-black">{finalPrice}</span>
                            </div>
                        </div>
                        <div className="mb-4">
                            <span className="font-bold text-black">Select Size: </span>
                            <div className="flex items-center mt-2">
                                    <input type="radio" id="option1" name="options" value="unitaire" checked={selectedOption === "unitaire"} onChange={handleOptionChange} className="hidden" />
                                    <label htmlFor="option1" className="bg-gray-300 text-gray-700 py-2 px-4 rounded-full font-bold mr-2 hover:bg-gray-400 cursor-pointer">À l'unité</label>

                                    <input type="radio" id="option2" name="options" value={`pack_${product?.nbProductBox}`} checked={selectedOption !== "unitaire"} onChange={handleOptionChange} className="hidden" />
                                    <label htmlFor="option2" className="bg-gray-300 text-gray-700 py-2 px-4 rounded-full font-bold mr-2 hover:bg-gray-400 cursor-pointer">Pack de {product?.nbProductBox}</label>
                                </div>
                        <div>
                            <span className="font-bold text-black">Product Description: </span>
                            <p className="text-black text-sm mt-2">
                                {product?.description}
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
);
};

export default FormProduct;